using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.ViewModels;
using Portal.DAL;

namespace Portal.PortalBL.DropDown
{
    public class DropDownEngine : IDropDownBL
    {
        public List<DropDownViewModal> GetList(string type, int? id, string ids = null, string search = null)
        {
            if (type == "county_dropdown" || type== "multi_country_dropdown")
            {
                return GetCountryList();
            }

            if (type == "state_dropdown" || type== "multi_state_dropdown")
            {
                return GetStateList(id,ids);
            }

            if (type == "city_dropdown" || type == "multi_city_dropdown")
            {
                return GetCityList(id,ids,search);
            }

            if (type == "experience_dropdown")
            {
                return GetCandidateExperienceList();
            }

            if (type == "expertise_dropdown")
            {
                return GetCandidateExpertiseList(search);
            }

            if (type == "vendor_dropdown")
            {
                return GetVendorList();
            }

            if(type == "plan_dropdown")
            {
                return GetPlanList();
            }

            return null;
        }

        private List<DropDownViewModal> GetCountryList()
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var country = _context.portal_country.Select(x => new DropDownViewModal
                {
                    name = x.country_name,
                    value = x.pk_country_id
                }).ToList();
                return country;
            }
        }
        private List<DropDownViewModal> GetStateList(int? id=null,string ids=null)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var state = _context.portal_get_states_by_country(id, ids).Select(x => new DropDownViewModal
                {
                    name = x.state_name,
                    value = x.pk_state_id
                }).ToList();

                return state;
            }
        }
        private List<DropDownViewModal> GetCityList(int? id = null, string ids = null, string search=null)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var city = _context.portal_get_cities_by_state(id,ids,search).Select(x => new DropDownViewModal
                {
                    name = x.city_name,
                    value = x.pk_city_id
                }).ToList();
                return city;
            }
        }

        private List<DropDownViewModal> GetCandidateExperienceList()
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var city = _context.portal_experience.Select(x => new DropDownViewModal
                {
                    name = x.experience_years + " ( " + x.level + ")",
                    value = x.pk_experience_level_id
                }).ToList();
                return city;
            }
        }

        private List<DropDownViewModal> GetCandidateExpertiseList(string search = null)
        {
            using (PortalEntities _context = new PortalEntities())
            {

                var temp_data = _context.portal_experise.ToList();
                var data = _context.portal_experise.Where(x => search == null || x.expertise_name.ToLower().Contains(search.ToLower())).Select(x => new DropDownViewModal
                {
                    name = x.expertise_name,
                    value = x.pk_expertise_id
                }).ToList();
                return data;
            }
        }

        private List<DropDownViewModal> GetVendorList()
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var city = _context.portal_user_role_mapping.Where(x => x.fk_role_id == 2).Select(x => new DropDownViewModal
                {
                    name = x.portal_user.firstname,
                    value = x.portal_user.pk_user_id
                }).ToList();
                return city;
            }
        }

        private List<DropDownViewModal> GetPlanList()
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_plan.Where(x => x.is_active == true && x.is_deleted==false).Select(x => new DropDownViewModal
                {
                    name = x.plan_name,
                    value = x.pk_plan_type_id
                }).ToList();
                return data;
            }
        }
    }
}