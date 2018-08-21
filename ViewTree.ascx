<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewTree.ascx.cs" Inherits="FreeSource.Modules.TabLocale.ViewTree" %>
<%@ Register Src="~/desktopmodules/FreeSource_TabLocale/controls/EditLocaleTab.ascx" TagPrefix="uc1" TagName="EditLocaleTab" %>

<div class="et_header">
    <asp:LinkButton ID="btnClearCache" runat="server" CssClass="et_button" resourcekey="btnClearCache">Clear Cache</asp:LinkButton>
    <asp:LinkButton ID="btnListView" runat="server" CssClass="et_button" resourcekey="btnListView">List View</asp:LinkButton>
</div>

<asp:UpdateProgress ID="updateProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
    <ProgressTemplate>
        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
            <img src="/images/loading.gif" alt="Loading..." title="Loading..." style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="et_tree">
            <div class="et_tree_sidebar">
                <asp:TreeView ID="tvPages" runat="server" ImageSet="Simple" ShowLines="true"
                    OnSelectedNodeChanged="tvPages_SelectedNodeChanged"
                    OnTreeNodeExpanded="tvPages_TreeNodeExpanded">
                    <NodeStyle Font-Size="12px" ForeColor="Black" HorizontalPadding="5px"
                        NodeSpacing="0px" VerticalPadding="0px" />
                    <SelectedNodeStyle ForeColor="White" BackColor="#828282" HorizontalPadding="5px" />
                </asp:TreeView>
            </div>
            <div class="et_tree_details<%=pnlCompare.Visible ? " compare-mode" : ""%>">
                <div class="head_row">
                    <asp:LinkButton ID="lbtnToggleCompare" runat="server" CssClass="et_button" />
                </div>
                <div class="tab-info">
                    <uc1:EditLocaleTab runat="server" id="ucEditLocaleTab" />
                </div>
                <div class="tab-info" id="pnlCompare" runat="server">
                    <uc1:EditLocaleTab runat="server" id="ucEditLocaleTabCompare" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
