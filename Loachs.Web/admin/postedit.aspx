<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_postedit" Title="无标题页" Codebehind="postedit.aspx.cs" %>
<%@ Import Namespace="Loachs.Common" %>
<%@ Import Namespace="Loachs.Business" %>
<%@ Import Namespace="Loachs.Entity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="stylesheet" href="../common/css/thickbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../common/scripts/jquery.js"></script>
<script type="text/javascript" src="../common/scripts/jquery.tagto.js"></script>
<script type="text/javascript" src="../common/scripts/thickbox.js"></script>
<script type="text/javascript" src="../common/editor/ckeditor.js"></script>
<style type="text/css">
/*tag choose*/
.selected {background:#c00; color:#fff;}

</style>
<script type="text/javascript">
        (function($){
            $(document).ready(function(){
                $("#taglist").tagTo("#<%=txtTags.ClientID %>",",","selected");
            });    
        })(jQuery);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2><%=headerTitle%></h2>
<%=ResponseMessage %>
    <p>
        <label class="label" for="<%=txtTitle.ClientID %>">标题:</label>
        <asp:TextBox ID="txtTitle"   Width="95%" runat="server" CssClass="text"></asp:TextBox>           
    </p>
    <p>
        <label class="label"  for="<%=txtContents.ClientID %>">
            正文:<span class="small gray"> </span>
            <a href="upfilebyeditor.aspx?keepThis=true&getfile=1&TB_iframe=true&height=480&width=780" title="插入图片/文件" class="thickbox" target="_blank">插入图片/文件</a>

        </label>
        <asp:TextBox ID="txtContents" runat="server" Height="400" TextMode="MultiLine"   Width="95%"></asp:TextBox>
        <label  class="label gray">提示：Enter产生&lt;p&gt;(换段), Shift+Enter产生&lt;br/&gt;(换行)</label>
    </p>
    
<script type="text/javascript">
function addFileToEditor(fileUrl,fileExtension)
{
    if(fileExtension=='.gif' || fileExtension=='.jpg' || fileExtension=='.jpeg' || fileExtension=='.bmp' || fileExtension=='.png'){
        var imageTag="<img src=\""+fileUrl+"\"/>";
        CKEDITOR.instances.<%=txtContents.ClientID %>.insertHtml(imageTag); 
    }else{
        var imageTag="<a href=\""+fileUrl+"\">"+fileUrl+"</a>";
        CKEDITOR.instances.<%=txtContents.ClientID %>.insertHtml(imageTag);
    }
    
}


function createSummary(type) {
    var ckContent =CKEDITOR.instances.<%=txtContents.ClientID %>;
    var ckSummary =CKEDITOR.instances.<%=txtSummary.ClientID %>;

    var iLength  = ckSummary.getData().length;

    var r=true;
    if(iLength>0){
        if(!confirm('提取将会覆盖已有摘要,要继续吗?')){
		    r=false;
        }
    }
    if(r){
        if(type =='full'){
            ckSummary.setData(ckContent.getData());  
        }
        else{
		    ckSummary.setData(ckContent.getData().replace(/<[^>]+>/g, "").substring(0,500));     //CK会自动处理未闭合的标签，我们不用多管它。要是标签被切了一半显示出来了自己编辑下就好。
		}
    }
    return false;
}
</script>
    <p>
        <label  class="label" for="<%=txtSummary.ClientID %>">摘要: <a href="javascript:void(0);" onclick="createSummary('full');">从正文提取全部</a>/<a href="javascript:void(0);" onclick="createSummary('part');">部分</a></label>
        <asp:TextBox ID="txtSummary" runat="server" Height="120px" TextMode="MultiLine"    Width="95%"></asp:TextBox>
   </p>

<script type="text/javascript">
    CKEDITOR.replace( '<%=txtContents.ClientID %>' ,
                        {
                            height:320,width:'95%'
                        }
                     );

    CKEDITOR.replace( '<%=txtSummary.ClientID %>',
				{
				    height:130,width:'95%',
					// Defines a simpler toolbar to be used in this sample.
					// Note that we have added out "MyButton" button here.
					toolbar : [ [ 'Source', '-', 'Bold', 'Italic', 'Underline', 'Strike','-','Link', '-' ] ]
				});
</script>

   <p>
        <label  class="label" for="<%=txtTags.ClientID %>">标签:<span  class="gray small"  > (多个标签用逗号隔开[选填])</span></label>
        <asp:TextBox ID="txtTags"  runat="server" Width="49%" CssClass="text"></asp:TextBox> 
        <a  href="###" onclick="showTag();" >查看常用标签↓</a>
        <div id="taglist" style=" border:2px solid #ccc;display:none;  line-height:135%; padding:3px ;  ">
            
            <%
                System.Collections.Generic.List<TagInfo> tagList = TagManager.GetTagList(20);
                foreach (TagInfo tag in  tagList)
              { %>
              
                  <a href="###" style="padding-left:3px;"><%=tag.Name %></a><span class="gray small">(<%=tag.Count %>)</span>
                   
            <%} %>
            
            <%if (tagList.Count == 0)
              { %>
              暂无
            <%} %>
            
        </div>
                 
<script type="text/javascript">
    function showTag(){
        if(document.getElementById('taglist').style.display==''){
            document.getElementById('taglist').style.display='none';
        }else{
            document.getElementById('taglist').style.display='';
        }
    }
</script>       
    </p>
    
    <p>
         <label  class="label" for="<%=txtCustomUrl.ClientID %>">别名:<span  class="gray small"  > (字母,数字,中文,连字符[选填])</span></label>
         <asp:TextBox ID="txtCustomUrl"    runat="server" Width="49%" CssClass="text"></asp:TextBox>
          
    </p>
    <p>
         <label  class="label" for="<%=ddlUrlType.ClientID %>">Url地址格式:</label>
         <asp:DropDownList ID="ddlUrlType" runat="server" Width="50%" CssClass="text"></asp:DropDownList>  
    </p>
    <p>
         <label  class="label" for="<%=ddlTemplate.ClientID %>">文章模板:<span  class="gray small"> (位于<%=ConfigHelper.SitePath %>themes/<%=setting.Theme %>/template/下,当然,您也可以自己制作,文件名必须以"post"开头)</span></label>
         <asp:DropDownList ID="ddlTemplate" runat="server" Width="50%"></asp:DropDownList>  
          
    </p>
    <p>
        <label  class="label" for="<%=ddlCategory.ClientID %>">分类:</label>
        <asp:DropDownList ID="ddlCategory" runat="server"  Width="50%" ></asp:DropDownList> 
    </p>
    <p>
        <asp:CheckBox ID="chkStatus" runat="server" Text="发布" Checked="true" />
        <asp:CheckBox ID="chkCommentStatus" runat="server" Text="允许评论" Checked="true" />
        <asp:CheckBox ID="chkRecommend" runat="server" Text="推荐"  />
        <asp:CheckBox ID="chkTopStatus" runat="server" Text="置顶"  />
        <asp:CheckBox ID="chkHideStatus" runat="server" Text="列表隐藏" ToolTip="不会出现在文章列表,常用于留言本,关于我的页面!"  />
        <asp:CheckBox ID="chkSaveImage" runat="server" Text="保存远程图片" ToolTip="自动将远程图片保存到本地，仅保存静态地址图片" />
    </p>
 
   <p> <asp:Button ID="btnEdit" runat="server" Text="添加" onclick="btnEdit_Click"  CssClass="button" /></p>

</asp:Content>

