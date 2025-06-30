using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlogApplication.Helpers
{
    [HtmlTargetElement("date-format")]
    public class DateFormatTagHelper : TagHelper
    {
        public DateTime Value { get; set; } 
        public string Format { get; set; } = "dd/MM/yyyy"; 

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span"; 
            output.Content.SetContent(Value.ToString(Format));
        }
    }
}
