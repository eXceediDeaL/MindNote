using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MindNote.Client.SDK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.TagHelpers
{
    [HtmlTargetElement("user-display")]
    public class UserDisplayTagHelper : TagHelper
    {
        public User Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("far");
                icon.AddCssClass("fa-user");
                output.Content.AppendHtml(icon);

                TagBuilder name = new TagBuilder("span");
                name.Attributes["style"] = "margin-left: 5px";
                name.InnerHtml.Append(Value.Name);
                output.Content.AppendHtml(name);
            }
            base.Process(context, output);
        }
    }
}
