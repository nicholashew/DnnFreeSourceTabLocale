<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="FreeSource.Modules.TabLocale.View" EnableViewState="true" %>

<div id="etcont">
    <div id="et_help">
        <asp:Label ID="lblhelp" runat="server" resourcekey="lblhelp" />
    </div>
    <table border="0" class="et_table">
        <colgroup>
            <col width="12%" />
            <col width="26%" />
            <col width="26%" />
            <col width="26%" />
            <col width="10%" />
        </colgroup>
        <thead>
            <tr>
                <th class="et_tabs" colspan="2"><span class="et_head">
                    <asp:Label ID="lblParent" runat="server" resourcekey="lblParent" /></span><asp:DropDownList CssClass="et_select" ID="ddlTabs" runat="server" AutoPostBack="true" /></th>
                <th class="et_tabs"><span class="et_head">
                    <asp:Label ID="lblLocale" runat="server" resourcekey="lblLocale" /></span>
                    <asp:DropDownList CssClass="et_selectL" ID="ddlLocale" runat="server" AutoPostBack="true" /></th>
                <th class="et_tabs"><span class="et_head">
                    <asp:Label ID="lblLocaleCompare" runat="server" resourcekey="lblLocaleCompare" /></span><asp:DropDownList CssClass="et_selectL" ID="ddlLocaleCompare" runat="server" AutoPostBack="true" /></th>
                <th class="et_tabs"></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="repTabs" runat="server">
                <ItemTemplate>
                    <tr class="et_items">
                        <td>
                            <asp:Label CssClass="et_desc" ID="lbl_TabName" runat="server" resourcekey="lblTabName" /></td>
                        <td>
                            <asp:Label CssClass="et_tabname" ID="lblTabName" runat="server" /></td>
                        <td>
                            <asp:TextBox CssClass="et_txtbox" ID="txtTabName" runat="server" /></td>
                        <td>
                            <asp:Label CssClass="et_comp" ID="lblCompareTabName" runat="server" /></td>
                        <td>
                            <asp:HiddenField ID="hfTabId" runat="server" />
                            <asp:LinkButton ID="btnUpdate" runat="server" CssClass="et_button" resourcekey="btnUpdate" Text="Update" CommandName="Update" />
                        </td>
                    </tr>
                    <tr class="et_items">
                        <td>
                            <asp:Label CssClass="et_desc" ID="lbl_TabTitle" runat="server" resourcekey="lblTabTitle" /></td>
                        <td>
                            <asp:Label CssClass="et_tabname" ID="lblTabTitle" runat="server" /></td>
                        <td>
                            <asp:TextBox CssClass="et_txtbox" ID="txtTabTitle" runat="server" /></td>
                        <td>
                            <asp:Label CssClass="et_comp" ID="lblCompareTabTitle" runat="server" /></td>
                        <td></td>
                    </tr>
                    <tr class="et_items">
                        <td>
                            <asp:Label CssClass="et_desc" ID="lbl_Description" runat="server" resourcekey="lblTabDescription" /></td>
                        <td>
                            <asp:Label CssClass="et_tabname" ID="lblTabDescription" runat="server" /></td>
                        <td>
                            <asp:TextBox CssClass="et_txtarea" ID="txtTabDescription" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        <td>
                            <asp:Label CssClass="et_comp" ID="lblCompareTabDescription" runat="server" /></td>
                        <td></td>
                    </tr>
                    <tr class="et_items">
                        <td>
                            <asp:Label CssClass="et_desc" ID="lbl_KeyWords" runat="server" resourcekey="lblTabKeyWords" /></td>
                        <td>
                            <asp:Label CssClass="et_tabname" ID="lblTabKeyWords" runat="server" /></td>
                        <td>
                            <asp:TextBox CssClass="et_txtarea" ID="txtTabKeyWords" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        <td>
                            <asp:Label CssClass="et_comp" ID="lblCompareTabKeyWords" runat="server" /></td>
                        <td></td>
                    </tr>
                    <tr class="et_items">
                        <td>
                            <asp:Label CssClass="et_desc" ID="lbl_PageHeadText" runat="server" resourcekey="lblTabPageHeadText" /></td>
                        <td>
                            <asp:Label CssClass="et_tabname" ID="lblTabPageHeadText" runat="server" /></td>
                        <td>
                            <asp:TextBox CssClass="et_txtbox" ID="txtTabPageHeadText" runat="server" /></td>
                        <td>
                            <asp:Label CssClass="et_comp" ID="lblCompareTabPageHeadText" runat="server" /></td>
                        <td></td>
                    </tr>
                </ItemTemplate>
                <SeparatorTemplate>
                    <tr class="et_items seperator">
                        <td colspan="5"></td>
                    </tr>
                </SeparatorTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="et_update">
        <asp:LinkButton ID="btnUpdateAll" runat="server" CssClass="et_button" resourcekey="btnUpdateAll" OnClick="btnUpdateAll_Click">Update All</asp:LinkButton>
    </div>
</div>

<script>
    /**
     * Cross-browser Document Ready
     */
    var domIsReady = (function (domIsReady) {
        var isBrowserIeOrNot = function () { return (!document.attachEvent || typeof document.attachEvent === "undefined" ? 'not-ie' : 'ie'); }
        domIsReady = function (callback) { if (callback && typeof callback === 'function') { if (isBrowserIeOrNot() !== 'ie') { document.addEventListener("DOMContentLoaded", function () { return callback(); }); } else { document.attachEvent("onreadystatechange", function () { if (document.readyState === "complete") { return callback(); } }); } } else { console.error('The callback is not a function!'); } }
        return domIsReady;
    })(domIsReady || {});

    (function (document, window, domIsReady, undefined) {
        domIsReady(function () {
            var unsaved = false;

            // Detect input changed
            document.querySelectorAll('#etcont input[type=text], #etcont textarea').forEach(function (el) {
                el.addEventListener('input', function (e) {
                    unsaved = true;
                });
            });

            // Detech AutoPostback for ASP.NET DropDownList
            document.querySelectorAll('#etcont select').forEach(function (el) {
                el.addEventListener("change", function (e) {
                    e.preventDefault();
                    return !unsaved;
                });
            });

            // Detect button link clicked
            document.querySelectorAll('#etcont .et_button').forEach(function (el) {
                el.onclick = function (e) {
                    unsaved = false;
                    return true;
                };
            });

            // Detect Form submit
            document.querySelector('form').addEventListener("submit", function (e) {
                unsaved = false;
                return true;
            });

            // Handle Form Unload 
            window.onbeforeunload = function (e) {
                e = e || window.event;
                if (unsaved) {
                    var message = 'You have unsaved changes on this page. Do you want to leave this page and discard your changes or stay on this page?';
                    // For IE and Firefox
                    if (e) {
                        e.returnValue = message;
                    }
                    // For Safari
                    return message;
                }
            };
        });
    })(document, window, domIsReady);
</script>
