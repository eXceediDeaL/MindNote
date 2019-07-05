using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MindNote.Server.Host.TagHelpers
{
    [HtmlTargetElement("text-clipper")]
    public class TextClipperTagHelper : TagHelper
    {
        public string Text { get; set; }

        public int Length { get; set; } = 300;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Text != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder content = new TagBuilder("span");
                string res;
                if (Text.Length <= Length)
                {
                    res = Text;
                }
                else
                {
                    res = Text.Substring(0, Length) + "...";
                }
                content.InnerHtml.Append(res);
                output.Content.AppendHtml(content);
            }
            base.Process(context, output);
        }
    }
}
