String.prototype.trim = function(){  
return this.replace(/(^\s*)|(\s*$)/g, "");  
}  

function checkComment(){
    var author=$("#commentauthor").val().trim();
    var email=$("#commentemail").val().trim();
    var siteurl=$("#commentsiteurl").val().trim();
    var content=$("#commentcontent").val().trim();
    
    var enableverifycode = $("#enableverifycode").val();
    var verifycode='';
    if (enableverifycode == 1) {
        verifycode=$("#commentverifycode").val().trim();
    }
   
    var r='';
    if(author==""){
        r=" 名称";
    }
    if(email==""){
        r+=" 邮箱";
    }
    if(content==""){
        r+=" 内容";
    }
    if (enableverifycode == 1) {
        if(verifycode==""){
            r+=" 验证码";
        }
    }
 
    
    if(r.length>0){
        $("#commentmessage").html('<div>请输入:' + r + '</div>');
        return false;
    } 
    return true;
}