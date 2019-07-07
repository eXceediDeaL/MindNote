using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MindNote.Server.Host.TagHelpers
{
    [HtmlTargetElement("read-time")]
    public class ReadTimeTagHelper : TagHelper
    {
        const int CharPerMinute = 500;

        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Text != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("far");
                icon.AddCssClass("fa-clock");
                output.Content.AppendHtml(icon);

                TagBuilder count = new TagBuilder("span");
                count.Attributes["style"] = "margin-left: 5px";
                int time = (int)Math.Round(Text.Length / (double)CharPerMinute);
                if (time <= 1)
                {
                    count.InnerHtml.Append($"1 minute");
                }
                else
                {
                    count.InnerHtml.Append($"{time.ToString()} minutes");
                }
                output.Content.AppendHtml(count);
            }
            base.Process(context, output);
        }
    }
}
