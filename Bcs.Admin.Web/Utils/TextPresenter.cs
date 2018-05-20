﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AngleSharp;
using AngleSharp.Parser.Html;
using BcsAdmin.BL.Dto;
using BcsResolver.Extensions;
using TextRange = BcsResolver.Syntax.Tokenizer.TextRange;

namespace Bcs.Analyzer.DemoWeb.Utils
{
    public class SpanPoint
    {
        public int Id { get; set; }
        public int Position { get; set; }

        public bool IsStart { get; set; }
        public string CssClass { get; set; }

        public string TooltipText { get; set; }
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

            int spanId = 0;
            foreach (var span in styleSpans)
            {
                if (!textRange.ContainsRange(span.Range))
                {
                    continue;
                }

                spanPoints.Add(new SpanPoint { Position = span.Range.Start, CssClass = span.CssClass, IsStart = true, Id = spanId, TooltipText = span.TooltipText });
                spanPoints.Add(new SpanPoint { Position = span.Range.End, CssClass = span.CssClass, IsStart = false, Id = spanId, TooltipText = span.TooltipText });
                spanId++;
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

                    richTextBuilder.Append(CreateHtmlStartTag(point));
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
                            richTextBuilder.Append(CreateHtmlStartTag(point));
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

        private static string CreateHtmlStartTag(SpanPoint point)
        {
            var tooltip = string.IsNullOrWhiteSpace(point.TooltipText)
                ? ""
                : $"data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{point.TooltipText}\"";
            return $"<span {tooltip} class=\"{point.CssClass}\">";
        }

        private int SpanPointComparison(SpanPoint left, SpanPoint right)
        {
            var position = left.Position.CompareTo(right.Position);

            var tagorder = left.IsStart
                    ? right.Id.CompareTo(left.Id)
                    : left.Id.CompareTo(right.Id);

            var startOrder = right.IsStart.CompareTo(left.IsStart);

            return
                position != 0 ? position :
                tagorder != 0 ? tagorder :
                startOrder != 0 ? startOrder : 0;

        }
    }
}