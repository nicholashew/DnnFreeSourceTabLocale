<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BreadcrumbSettings.ascx.cs" Inherits="FreeSource.Modules.TabLocale.BreadcrumbSettings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnForm dnnEditBasicSettings" id="dnnEditBasicSettings">
    <div class="dnnFormExpandContent dnnRight "><a href=""><%=LocalizeString("ExpandAll")%></a></div>
    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead dnnClear">
        <a href="" class="dnnSectionExpanded">
            <%=LocalizeString("BasicSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label ID="lblHeaderTemplate" runat="server" />
            <asp:TextBox ID="txtHeaderTemplate" runat="server" TextMode="MultiLine" Rows="4" Columns="20" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblItemTemplate" runat="server" />
            <asp:TextBox ID="txtItemTemplate" runat="server" TextMode="MultiLine" Rows="4" Columns="20" 
                placeholder="Example: &#x3C;li&#x3E;&#x3C;a href=&#x22;[url]&#x22;&#x3E;[name]&#x3C;/a&#x3E;&#x3C;/li&#x3E;" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblSeperatorTemplate" runat="server" />
            <asp:TextBox ID="txtSeperatorTemplate" runat="server" TextMode="MultiLine" Rows="4" Columns="20" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblFooterTemplate" runat="server" />
            <asp:TextBox ID="txtFooterTemplate" runat="server" TextMode="MultiLine" Rows="4" Columns="20" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblEmptyTemplate" runat="server" />
            <asp:TextBox ID="txtEmptyTemplate" runat="server" TextMode="MultiLine" Rows="4" Columns="20" />
        </div>
    </fieldset>
</div>
<asp:LinkButton ID="btnSubmit" runat="server" resourcekey="btnSubmit" CssClass="dnnPrimaryAction" />
<asp:LinkButton ID="btnCancel" runat="server" resourcekey="btnCancel" CssClass="dnnSecondaryAction" />

<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function dnnEditBasicSettings() {
            $('#dnnEditBasicSettings').dnnPanels();
            $('#dnnEditBasicSettings .dnnFormExpandContent a').dnnExpandAll({
                expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>',
                collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>',
                targetArea: '#dnnEditBasicSettings'
            });
        }

        $(document).ready(function () {
            dnnEditBasicSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                dnnEditBasicSettings();
            });
        });
    }(jQuery, window.Sys));
</script>
