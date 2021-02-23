using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Portal.Common;
using Portal.DAL;
using Portal.PortalBL.RecruiterBL;
using Portal.PortalBL.RequirementBL;
using Portal.PortalBL.UserBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.ImportExcel
{
    public class ImportEngine : IImportBL
    {
        public ResponseOut UploadVendorCandidate(ImportVendorCandidateViewModel data)
        {

            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut parentResponseOut = new ResponseOut();
                try
                {
                    ResponseOut vendorResponseOut = new ResponseOut();
                    //Insert or  Get Country
                    int country_id = _context.portal_country.Where(x => x.country_name.ToLower() == data.country.ToLower()).Select(x => x.pk_country_id).FirstOrDefault();
                    int state_id = _context.portal_state.Where(x => x.state_name.ToLower() == data.state.ToLower()).Select(x => x.pk_state_id).FirstOrDefault();
                    int city_id = _context.portal_city.Where(x => x.city_name.ToLower() == data.city.ToLower()).Select(x => x.pk_city_id).FirstOrDefault();
                    int vendor_id = _context.portal_user.Where(x => x.user_name.ToLower() == data.vendor_user_name.ToLower()).Select(x => x.pk_user_id).FirstOrDefault();
                    int experience_level_id = _context.portal_experience.Where(x => x.level.ToLower() == data.experience_level.ToLower()).Select(x => x.pk_experience_level_id).FirstOrDefault();
                    string technology_id = _context.portal_experise.Where(x => x.expertise_name.ToLower() == data.candidate_technology.ToLower()).Select(x => x.pk_expertise_id.ToString()).FirstOrDefault();
                    if (country_id == 0)
                    {
                        portal_country _country = new portal_country();
                        _country.country_name = data.country;
                        _context.portal_country.Add(_country);
                        _context.SaveChanges();
                        country_id = _country.pk_country_id;
                    }
                    if (state_id == 0)
                    {
                        portal_state _state = new portal_state();
                        _state.state_name = data.state;
                        _state.fk_country_id = country_id;
                        _context.portal_state.Add(_state);
                        _context.SaveChanges();
                        state_id = _state.pk_state_id;
                    }
                    if (city_id == 0)
                    {
                        portal_city _city = new portal_city();
                        _city.city_name = data.city;
                        _city.fk_state_id = state_id;
                        _context.portal_city.Add(_city);
                        _context.SaveChanges();
                        city_id = _city.pk_city_id;
                    }
                    if (vendor_id == 0)
                    {
                        UserViewModel _user = new UserViewModel();
                        IUserBL _userbl = new UserEngine();
                        _user.user_name = data.vendor_user_name;
                        _user.password = data.vendor_password;
                        _user.firstname = data.vendor_name;
                        _user.fk_country_id = country_id;
                        _user.fk_state_id = state_id;
                        _user.fk_city_id = city_id;
                        _user.fk_user_type = 2;
                        vendorResponseOut = _userbl.AddUserProfile(_user);
                    }
                    else
                    {
                        vendorResponseOut.trnId = vendor_id;
                    }
                    if (string.IsNullOrEmpty(technology_id))
                    {
                        portal_experise _expertise = new portal_experise();
                        _expertise.expertise_name = data.candidate_technology;
                        _context.portal_experise.Add(_expertise);
                        _context.SaveChanges();
                        technology_id = _expertise.pk_expertise_id.ToString();
                    }
                    //Recruiter name have to change Candidate
                    IRecruiterBL _candidatebl = new RecruiterEngine();
                    RecruiterViewModel _candidate = new RecruiterViewModel();
                    _candidate.firstname = data.candidate_firstname;
                    _candidate.expertise_profession = technology_id;
                    _candidate.about_us = data.candidate_one_liner_headline;
                    _candidate.fk_experience_level = experience_level_id;
                    _candidate.fk_country_id = country_id;
                    _candidate.fk_state_id = state_id;
                    _candidate.fk_city_id = city_id;
                    _candidate.availability = data.availability;
                    _candidatebl.AddUpdateRecruiter(_candidate, vendorResponseOut.trnId);
                    parentResponseOut.status = ActionStatus.Success;
                    parentResponseOut.status = ActionMessage.RecordSaved;

                }

                catch (Exception ex)
                {
                    parentResponseOut.status = ActionStatus.Fail;
                    parentResponseOut.message = ActionMessage.ApplicationException;
                }

                return parentResponseOut;
            }
        }
        public ResponseOut UploadPostRequirement(PostRequirementImportViewModel data)
        {
            IRequirementBL _requirement = new RequirementEngine();
            return _requirement.PostRequirementUpload(data);
        }
        public string ImportData(string Identifier, string type, string name)
        {
            try
            {
                string File = "";
                //name=name.Split()

                File = HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["FileUploadSection"].Replace('/', '\\') + name;

                DataTable dt = new DataTable();
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(File, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string relationshipId = sheets.First().Id.Value;

                    dt.Columns.Clear();
                    dt.Rows.Clear();
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    foreach (Cell cell in rows.ElementAt(0))
                    {
                        dt.Columns.Add(Utilities.GetCellValue(spreadSheetDocument, cell));
                    }

                    foreach (Row row in rows) //this will also include your header row...
                    {
                        DataRow tempRow = dt.NewRow();
                        int columnIndex = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            // Gets the column index of the cell with data
                            int cellColumnIndex = (int)Utilities.GetColumnIndexFromName(Utilities.GetColumnName(cell.CellReference));
                            cellColumnIndex--; //zero based index
                            if (columnIndex < cellColumnIndex)
                            {
                                do
                                {
                                    tempRow[columnIndex] = ""; //Insert blank data here;
                                    columnIndex++;
                                }
                                while (columnIndex < cellColumnIndex);
                            }
                            tempRow[columnIndex] = Utilities.GetCellValue(spreadSheetDocument, cell);

                            columnIndex++;
                        }
                        dt.Rows.Add(tempRow);
                    }
                    dt.Rows.RemoveAt(0); // Remove oth element.

                    foreach (DataRow row in dt.Rows)
                    {
                        if (type == "post_requirement")
                        {
                            var data = new PostRequirementImportViewModel
                            {
                                client_name = row["Client Name"].ToString(),
                                contact_details = row["contact details"].ToString(),
                                email_id = row["email id"].ToString(),
                                engagement_model = row["Engagement Model"].ToString(),
                                location = row["Location"].ToString(),
                                requirement_description = row["Requirement description"].ToString(),
                                requirement_title = row["Requirement Title"].ToString(),
                            };
                            if(data!=null)
                            {
                                UploadPostRequirement(data);
                            }
                        }
                        else
                        {
                            var data = new ImportVendorCandidateViewModel
                            {
                                vendor_name = row["Vendor name"].ToString(),
                                vendor_user_name = row["Vendor user name"].ToString(),
                                vendor_password = row["Vendor Password"].ToString(),
                                candidate_firstname = row["Candidate name"].ToString(),
                                candidate_one_liner_headline = row["one liner headline"].ToString(),
                                candidate_technology = row["Technology"].ToString(),
                                country = row["Country"].ToString(),
                                state = row["State"].ToString(),
                                city = row["City"].ToString(),
                                availability = row["Availability (within days)"].ToString(),
                                experience_level = row["Experience level"].ToString(),
                            };
                            UploadVendorCandidate(data);
                        }

                        //AddDistributor(Identifier, null, row);
                    }
                }

                return "Success";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseOut ImportVendorCandidate(string url)
        {
            throw new NotImplementedException();
        }
    }
}