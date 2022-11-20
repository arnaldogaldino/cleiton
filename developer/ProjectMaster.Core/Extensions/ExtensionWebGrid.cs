using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Web.Helpers;
using System.Web;

namespace ProjectMaster.Core.Extensions
{
    public class WebGrid : System.Web.Helpers.WebGrid
    {
        public WebGrid(IEnumerable<dynamic> source = null,
                       IEnumerable<string> columnNames = null,
                       string defaultSort = null,
                       int rowsPerPage = 50,
                       bool canPage = true,
                       bool canSort = true,
                       string ajaxUpdateContainerId = null,
                       string ajaxUpdateCallback = "jQueryTableStyling",
                       string fieldNamePrefix = null,
                       string pageFieldName = null,
                       string selectionFieldName = null,
                       string sortFieldName = null,
                       string sortDirectionFieldName = null)
        {
            
        }

        public IHtmlString GetHtml(string tableStyle = "webgrid", string headerStyle = "webgrid-header", string footerStyle = "webgrid-footer", string rowStyle = "webgrid-rows", string alternatingRowStyle = "webgrid-alternating-rows", string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, WebGridPagerModes mode = WebGridPagerModes.Numeric | WebGridPagerModes.NextPrevious, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5, object htmlAttributes = null)
        {
            return base.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle, selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, mode, firstText, previousText, nextText, lastText, numericLinksCount, htmlAttributes);
        }

    }


    //public class WebGrid<T> : WebGrid
    //{
    //    public WebGrid(IEnumerable<dynamic> source = null, 
    //                   IEnumerable<string> columnNames = null, 
    //                   string defaultSort = null, 
    //                   int rowsPerPage = 50, 
    //                   bool canPage = true, 
    //                   bool canSort = true, 
    //                   string ajaxUpdateContainerId = null,
    //                   string ajaxUpdateCallback = "jQueryTableStyling", 
    //                   string fieldNamePrefix = null, 
    //                   string pageFieldName = null, 
    //                   string selectionFieldName = null, 
    //                   string sortFieldName = null, 
    //                   string sortDirectionFieldName = null)
    //    { }

    //    public IHtmlString GetHtml(string tableStyle = "webgrid", string headerStyle = "webgrid-header", string footerStyle =  "webgrid-footer", string rowStyle = "webgrid-rows", string alternatingRowStyle = "webgrid-alternating-rows", string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, WebGridPagerModes mode = WebGridPagerModes.Numeric | WebGridPagerModes.NextPrevious, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5, object htmlAttributes = null)
    //    {
    //        return base.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle, selectedRowStyle, caption , displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, mode, firstText, previousText, nextText, lastText, numericLinksCount, htmlAttributes);
    //    }

    //    public WebGridColumn Column(
    //                string columnName = null,
    //                string header = null,
    //                Func<T, object> format = null,
    //                string style = null,
    //                bool canSort = true)
    //    {
             
    //        Func<dynamic, object> wrappedFormat = null;
    //        if (format != null)
    //        {
    //            wrappedFormat = o => format((T)o.Value);
    //        }
    //        WebGridColumn column = base.Column(
    //                      columnName, header,
    //                      wrappedFormat, style, canSort);
    //        return column;
    //    }
    //    public WebGrid<T> Bind(
    //            IEnumerable<T> source,
    //            IEnumerable<string> columnNames = null,
    //            bool autoSortAndPage = true,
    //            int rowCount = -1)
    //    {
    //        base.Bind(
    //             source.Cast<object>(),
    //             columnNames,
    //             autoSortAndPage,
    //             rowCount);
    //        return this;
    //    }
    //}
}
