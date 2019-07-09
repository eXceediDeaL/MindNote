using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MindNote.Frontend.Server.TagHelpers
{
    [HtmlTargetElement("item-time")]
    public class ItemTimeTagHelper : TagHelper
    {
        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (CreationTime != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("far");
                icon.AddCssClass("fa-calendar");
                output.Content.AppendHtml(icon);

                TagBuilder time = new TagBuilder("time");
                time.Attributes["style"] = "margin-left: 5px";
                time.Attributes["datetime"] = CreationTime.ToString();
                time.Attributes["title"] = string.Format("Creation time: {0} / Modification time: {1}", CreationTime.DateTime.ToString("yyyy-MM-dd hh:mm:ss"), ModificationTime.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                time.InnerHtml.Append(CreationTime.Date.ToString("yyyy-MM-dd"));
                output.Content.AppendHtml(time);
            }
            base.Process(context, output);
        }
    }
}
