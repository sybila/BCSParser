using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AngleSharp;
using AngleSharp.Parser.Html;
using BcsResolver.Extensions;
using TextRange = BcsResolver.Syntax.Tokenizer.TextRange;

namespace Bcs.Analyzer.DemoWeb.Utils
{
    public class StyleSpan
    {
        public TextRange Range { get; set; }
        public string CssClass { get; set; }
    }

    public class SpanPoint
    {
        public int Position { get; set; }

        public bool IsStart { get; set; }
        public string CssClass { get; set; }
    }

    public class TextPresenter
    {
        public string ToRawText(string richText)
        {
            var sanitizer = new HtmlParser();

            return sanitizer.Parse(richText).DocumentElement.TextContent;
        }

        public string CreateRichText(string rawText, List<StyleSpan> styleSpans)
        {
            var spanPoints = new List<SpanPoint>();
            var textRange = new TextRange(0, rawText.Length);

            foreach (var span in styleSpans)
            {
                if (!textRange.ContainsRange(span.Range))
                {
                    continue;
                }

                spanPoints.Add(new SpanPoint {Position =  span.Range.Start, CssClass = span.CssClass, IsStart = true});
                spanPoints.Add(new SpanPoint { Position = span.Range.End, CssClass = span.CssClass, IsStart = false });
            }

            spanPoints.Sort(SpanPointComparison);

            var previousPosition = 0;
            var richTextBuilder = new StringBuilder();

            var openSpans = new Stack<string>();

            foreach (var point in spanPoints)
            {
                var textChunk = WebUtility.HtmlEncode(rawText.Substring(previousPosition, point.Position - previousPosition));

                richTextBuilder.Append(textChunk);

                if (point.IsStart)
                {
                    openSpans.Push(point.CssClass);
                    richTextBuilder.Append($"<span class=\"{point.CssClass}\">");
                }
                else
                {
                    if (openSpans.Peek() == point.CssClass)
                    {
                        openSpans.Pop();
                        richTextBuilder.Append("</span>");
                    }
                    else
                    {
                        var closedSpans = new Stack<string>();
                        while (openSpans.Peek() != point.CssClass)
                        {
                            richTextBuilder.Append("</span>");
                            closedSpans.Push(openSpans.Pop());
                        }
                        openSpans.Pop();
                        richTextBuilder.Append("</span>");
                        while (closedSpans.Count > 0)
                        {
                            richTextBuilder.Append($"<span class=\"{closedSpans.Peek()}\">");
                            openSpans.Push(closedSpans.Pop());
                        }
                    }
                }

                previousPosition = point.Position;
            }
            var lastChunk = WebUtility.HtmlEncode(rawText.Substring(previousPosition, rawText.Length - previousPosition));
            richTextBuilder.Append(lastChunk);

            return richTextBuilder.ToString();
        }

        private int SpanPointComparison(SpanPoint left, SpanPoint right)
        {
            return left.Position.CompareTo(right.Position);
        }
    }
}