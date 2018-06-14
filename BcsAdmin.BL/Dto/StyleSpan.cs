using TextRange = BcsResolver.Syntax.Tokenizer.TextRange;

namespace BcsAdmin.BL.Dto
{
    public class StyleSpan
    {
        public TextRange Range { get; set; }
        public string CssClass { get; set; }
        public string TooltipText { get; set; }
        public string Category { get; set; }
    }
}