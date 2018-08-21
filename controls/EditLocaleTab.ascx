<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditLocaleTab.ascx.cs" Inherits="FreeSource.Modules.TabLocale.controls.EditLocaleTab" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>

<fieldset>
    <div class="dnnFormItem">
        <dnn:label id="lblLanguage" runat="server" />
        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="et_select" 
            AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTabName" runat="server" />
        <asp:TextBox ID="txtTabName" runat="server" CssClass="et_txtbox" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTabTitle" runat="server" />
        <asp:TextBox ID="txtTabTitle" runat="server" CssClass="et_txtbox" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTabDescription" runat="server" />
        <asp:TextBox ID="txtTabDescription" runat="server" TextMode="MultiLine" Rows="2" CssClass="et_txtarea" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTabKeyWords" runat="server" />
        <asp:TextBox ID="txtTabKeyWords" runat="server" TextMode="MultiLine" Rows="2" CssClass="et_txtarea" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblTabPageHeadText" runat="server" />
        <asp:TextBox ID="txtTabPageHeadText" runat="server" CssClass="et_txtbox" />
    </div>
    <div class="dnnFormItem" style="text-align:center">
        <asp:LinkButton ID="lbtnSave" runat="server" resourcekey="lbtnSave" OnClick="lbtnSave_Click" CssClass="et_button">Save</asp:LinkButton>
        <asp:LinkButton ID="lbtnDelete" runat="server" resourcekey="lbtnDelete" OnClick="lbtnDelete_Click" CssClass="et_button"
            OnClientClick="return confirm('Are your sure?');">Delete</asp:LinkButton>
    </div>
</fieldset>
