using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MindNote.Frontend.Server.TagHelpers
{
    [HtmlTargetElement("word-count")]
    public class WordCountTagHelper : TagHelper
    {
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Text != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("far");
                icon.AddCssClass("fa-file-word");
                output.Content.AppendHtml(icon);

                TagBuilder count = new TagBuilder("span");
                count.Attributes["style"] = "margin-left: 5px";
                string countRes;
                if (Text.Length < 1000)
                {
                    countRes = Text.Length.ToString();
                }
                else
                {
                    countRes = $"{(Text.Length / 1000.0).ToString("f1")}k";
                }
                count.InnerHtml.Append(countRes);
                output.Content.AppendHtml(count);
            }
            base.Process(context, output);
        }
    }
}
