/**
 * Created by Zhangyx on 2015/10/15.
 */
;(function($){
    var Caroursel = function (caroursel){
        var self = this;
        this.caroursel = caroursel;
        this.rotatorList = caroursel.find(".rotator-list");
        this.rotatorItems = caroursel.find(".rotator-item");
        this.firstrotatorItem = this.rotatorItems.first();
        this.lastrotatorItem = this.rotatorItems.last();
        this.prevBtn = this.caroursel.find(".rotator-prev-btn");
        this.nextBtn = this.caroursel.find(".rotator-next-btn");
        //Ä¬ÈÏ²ÎÊý
        this.setting = {
            "width":"1000",
            "height":"270",
            "rotatorWidth":"640",
            "rotatorHeight":"270",
            "scale":"0.8",
            "speed":"1000",
            "isAutoplay":"true",
            "dealy":"1000"
        }
        //×Ô¶¨Òå²ÎÊýÓëÄ¬ÈÏ²ÎÊýºÏ²¢
        $.extend(this.setting,this.getSetting())
        //ÉèÖÃµÚÒ»Ö¡Î»ÖÃ
        this.setFirstPosition();
        //ÉèÖÃÊ£ÓàÖ¡µÄÎ»ÖÃ
        this.setSlicePosition();
        //Ðý×ª
        this.rotateFlag = true;
        this.prevBtn.bind("click",function(){
            if(self.rotateFlag){
                self.rotateFlag = false;
                self.rotateAnimate("left")
            }
        });
        this.nextBtn.bind("click",function(){
            if(self.rotateFlag){
                self.rotateFlag = false;
                self.rotateAnimate("right")
            }
        });
        if(this.setting.isAutoplay){
            this.autoPlay();
            this.caroursel.hover(function(){clearInterval(self.timer)},function(){self.autoPlay()})
        }
    };
    Caroursel.prototype = {
        autoPlay:function(){
          var that = this;
          this.timer =  window.setInterval(function(){
              that.prevBtn.click();
          },that.setting.dealy)
        },
        rotateAnimate:function(type){
            var that = this;
            var zIndexArr = [];
            if(type == "left"){//Ïò×óÒÆ¶¯
                this.rotatorItems.each(function(){
                   var self = $(this),
                    prev = $(this).next().get(0)?$(this).next():that.firstrotatorItem,
                    width = prev.css("width"),
                    height = prev.css("height"),
                    zIndex = prev.css("zIndex"),
                    opacity = prev.css("opacity"),
                    left = prev.css("left"),
                    top = prev.css("top");
                    zIndexArr.push(zIndex);
                    self.animate({
                        "width":width,
                        "height":height,
                        "left":left,
                        "opacity":opacity,
                        "top":top,
                    },that.setting.speed,function(){
                        that.rotateFlag = true;
                    });
                });
                this.rotatorItems.each(function(i){
                    $(this).css("zIndex",zIndexArr[i]);
                });
            }
            if(type == "right"){//ÏòÓÒÒÆ¶¯
                this.rotatorItems.each(function(){
                    var self = $(this),
                    next = $(this).prev().get(0)?$(this).prev():that.lastrotatorItem,
                        width = next.css("width"),
                        height = next.css("height"),
                        zIndex = next.css("zIndex"),
                        opacity = next.css("opacity"),
                        left = next.css("left"),
                        top = next.css("top");
                        zIndexArr.push(zIndex);
                    self.animate({
                        "width":width,
                        "height":height,
                        "left":left,
                        "opacity":opacity,
                        "top":top,
                    },that.setting.speed,function(){
                        that.rotateFlag = true;
                    });
                });
                this.rotatorItems.each(function(i){
                    $(this).css("zIndex",zIndexArr[i]);
                });
            }
        },
        setFirstPosition:function(){
            this.caroursel.css({"width":this.setting.width,"height":this.setting.height});
            this.rotatorList.css({"width":this.setting.width,"height":this.setting.height});
            var width = (this.setting.width - this.setting.rotatorWidth) / 2;
            //ÉèÖÃÁ½¸ö°´Å¥µÄÑùÊ½
            this.prevBtn.css({"width":width , "height":this.setting.height,"zIndex":Math.ceil(this.rotatorItems.size()/2)});
            this.nextBtn.css({"width":width , "height":this.setting.height,"zIndex":Math.ceil(this.rotatorItems.size()/2)});
            this.firstrotatorItem.css({
                "width":this.setting.rotatorWidth,
                "height":this.setting.rotatorHeight,
                "left":width,
                "zIndex":Math.ceil(this.rotatorItems.size()/2),
                "top":this.setVertialType(this.setting.rotatorHeight)
            });
        },
        setSlicePosition:function(){
            var _self = this;
            var sliceItems = this.rotatorItems.slice(1),
                level = Math.floor(this.rotatorItems.length/2),
                leftItems = sliceItems.slice(0,level),
                rightItems = sliceItems.slice(level),
                rotatorWidth = this.setting.rotatorWidth,
                rotatorHeight = this.setting.rotatorHeight,
                Btnwidth = (this.setting.width - this.setting.rotatorWidth) / 2,
                gap = Btnwidth/level,
                containerWidth = this.setting.width;
            //ÉèÖÃ×ó±ßÖ¡µÄÎ»ÖÃ
            var i = 1;
            var leftWidth = rotatorWidth;
            var leftHeight = rotatorHeight;
            var zLoop1 = level;
            leftItems.each(function(index,item){
                leftWidth = rotatorWidth * _self.setting.scale;
                leftHeight = rotatorHeight*_self.setting.scale;
                $(this).css({
                    "width":leftWidth,
                    "height":leftHeight,
                    "left": Btnwidth - i*gap,
                    "zIndex":zLoop1--,
                    "opacity":1/(i+1),
                    "top":_self.setVertialType(leftHeight)
                });
                i++;
            });
            //ÉèÖÃÓÒÃæÖ¡µÄÎ»ÖÃ
            var j = level;
            var zLoop2 = 1;
            var rightWidth = rotatorWidth;
            var rightHeight = rotatorHeight;
            rightItems.each(function(index,item){
                var rightWidth = rotatorWidth * _self.setting.scale;
                var rightHeight = rotatorHeight*_self.setting.scale;
                $(this).css({
                    "width":rightWidth,
                    "height":rightHeight,
                    "left": containerWidth -( Btnwidth - j*gap + rightWidth),
                    "zIndex":zLoop2++,
                    "opacity":1/(j+1),
                    "top":_self.setVertialType(rightHeight)
                });
                j--;
            });
        },
        getSetting:function(){
            var settting = this.caroursel.attr("data-setting");
            if(settting.length > 0){
                return $.parseJSON(settting);
            }else{
                return {};
            }
        },
        setVertialType:function(height){
            var algin = this.setting.algin;
            if(algin == "top") {
                return 0
            }else if(algin == "middle"){
                return (this.setting.rotatorHeight - height) / 2
            }else if(algin == "bottom"){
                return this.setting.rotatorHeight - height
            }else {
                return (this.setting.rotatorHeight - height) / 2
            }
        }
    }
    Caroursel.init = function (caroursels){
        caroursels.each(function(index,item){
            new Caroursel($(this));
        })  ;
    };
    window["Caroursel"] = Caroursel;
})(jQuery)
