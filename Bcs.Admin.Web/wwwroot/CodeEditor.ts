/// <reference path="lib/knockout/knockout.d.ts" />
/// <reference path="lib/jquery/jquery.d.ts" />
/// <reference path="lib/dotvvm/dotvvm.d.ts" />

class StyleSpan {
    public Range: TextRange;
    public CssClass: string;
    public TooltipText: string;
}

class SpanPoint {
    public Id: number;
    public Position: number;
    public IsStart: boolean;
    public CssClass: string;
    public TooltipText: string;
}

class StringBuilder {
    private chunks: string[] = [];
    public append(chunk: string) {
        this.chunks.push(chunk);
    }

    public toString(): string {
        return this.chunks.join("");
    }
}

class TextRange {
    public Start: number;
    public Length: number;
    public End: number;

    public constructor(start: number, length: number) {
        this.Start = start;
        this.Length = length;
        this.End = start + length;
    }

    public ContainsPosition(position: number): boolean {
        return this.Start <= position && position < this.End;
    }

    public ContainsRange(range: TextRange): boolean {
        return this.ContainsPosition(range.Start) && range.End <= this.End;
    }
}

class TextPresenter {
    public CreateRichText = (rawText: string, styleSpans: StyleSpan[]): string => {
        rawText = rawText == null ? "" : rawText;
        var spanPoints: SpanPoint[] = [];
        var textRange = new TextRange(0, rawText.length);
        var spanId: number = 0;

        styleSpans.forEach((span => {
            if (!textRange.ContainsRange(span.Range)) {
                return;
            }
            spanPoints.push({ Position: span.Range.Start, CssClass: span.CssClass, IsStart: true, Id: spanId, TooltipText: span.TooltipText });
            spanPoints.push({ Position: span.Range.End, CssClass: span.CssClass, IsStart: false, Id: spanId, TooltipText: span.TooltipText });
            spanId++;
        }));

        spanPoints.sort(this.spanPointComparison);

        var previousPosition: number = 0;
        var richTextBuilder: StringBuilder = new StringBuilder();
        var openSpans: string[] = [];
        spanPoints.forEach(point => {
            var textChunk = rawText.substring(previousPosition, point.Position);
            richTextBuilder.append(textChunk);
            if (point.IsStart) {
                openSpans.push(point.CssClass);
                richTextBuilder.append(this.createHtmlStartTag(point));
            }
            else {
                if (openSpans[openSpans.length - 1] === point.CssClass) {
                    openSpans.pop();
                    richTextBuilder.append("</span>");
                }
                else {
                    var closedSpans: string[] = [];
                    while (openSpans[openSpans.length - 1] != point.CssClass) {
                        richTextBuilder.append("</span>");
                        closedSpans.push(openSpans.pop());
                    }
                    openSpans.pop();
                    richTextBuilder.append("</span>");
                    while (closedSpans.length > 0) {
                        richTextBuilder.append(this.createHtmlStartTag(point));
                        openSpans.push(closedSpans.pop());
                    }
                }
            }
            previousPosition = point.Position;
        });
        var lastChunk = rawText.substring(previousPosition, rawText.length);
        richTextBuilder.append(lastChunk);
        return richTextBuilder.toString();
    }

    private createHtmlStartTag = (point: SpanPoint): string => {
        var tooltip =
            (point.TooltipText == null || point.TooltipText === "")
                ? ""
                : "data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"" + point.TooltipText + "\"";

        return "<span " + tooltip + " class=\"" + point.CssClass + "\">";
    }

    private spanPointComparison = (left: SpanPoint, right: SpanPoint): number => {
        var position = this.intComparision(left.Position, right.Position);
        var tagorder = left.IsStart ? this.intComparision(right.Id, left.Id) : this.intComparision(left.Id, right.Id);
        var startOrder = this.boolComparision(right.IsStart, left.IsStart);
        return position !== 0 ? position : tagorder !== 0 ? tagorder : startOrder != 0 ? startOrder : 0;
    }

    private intComparision(a: number, b: number): number {
        return a - b;
    }

    private boolComparision(a: boolean, b: boolean): number {
        if (b > a) return -1;
        if (b < a) return 1;
        return 0;
    }
}

class CarretHelper {
    public static getCaretPosition = (node: Node) => {
        let caretPos: number = 0;
        let sel: Selection;
        let range: Range;

        const doc = document as any;

        if (window.getSelection) {
            sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0);
                return CarretHelper.getCharacterOffsetWithin(range, node);
            }
        } else if (doc.selection && doc.createRange) {
            range = doc.selection.createRange();
            return CarretHelper.getCharacterOffsetWithin(range, node);
        }
        return caretPos;
    }

    private static getCharacterOffsetWithin = (range: Range, node: Node) => {
        var filterFunction =
            node => {
                var nodeRange = document.createRange();
                nodeRange.selectNode(node);
                return nodeRange.compareBoundaryPoints(Range.END_TO_END, range) < 1
                    ? NodeFilter.FILTER_ACCEPT
                    : NodeFilter.FILTER_REJECT;
            }

        var treeWalker = document.createTreeWalker(
            node,
            NodeFilter.SHOW_TEXT,
            {
                acceptNode: filterFunction
            },
            false
        );

        var charCount = 0;
        while (treeWalker.nextNode()) {
            charCount += treeWalker.currentNode.textContent.length;
        }
        if (range.startContainer.nodeType === 3) {
            charCount += range.startOffset;
        }
        return charCount;
    }
}

class CodeEditor {

    private presenter: TextPresenter = new TextPresenter();
    private carretSet: boolean = false;

    public setCaret = (e, cp) => {
        this.carretSet = false;
        this.setCaretCore(e, cp);
    }

    private setCaretCore = (element: Node, caretPosition: number) => {
        var charsInNode = 0;

        (element.childNodes as any).forEach(
            (item: Node, index: number) => {
                if (this.carretSet) {
                    return 0;
                }

                if (item.nodeType === 3) {
                    var textContentLenght = item.textContent.length;
                    var relativeCaretPosition = caretPosition - charsInNode;

                    if (0 <= relativeCaretPosition && relativeCaretPosition <= textContentLenght) {
                        this.placeCaret(item, relativeCaretPosition);
                        this.carretSet = true;
                    }
                    else {
                        charsInNode += textContentLenght
                    }
                    return;
                }
                else if (item.nodeType === 1) {
                    charsInNode += this.setCaretCore(item, caretPosition - charsInNode)
                }
            });

        if (this.carretSet) {
            return 0;
        }
        return charsInNode;
    }

    private placeCaret = (item: Node, position: number): void => {
        var range = document.createRange();
        range.setStart(item, position);
        range.setEnd(item, position);

        var sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
    }

    public registerKeyHandle = () => {
        $(".code-editor").on("keydown", (e) => {
            var elem: any = e.target;
            this.handleKey(elem);
        });
    }

    private getVisibleTextFromTree = (node: Node) => {
        if (node.nodeType === Node.TEXT_NODE) { return node.textContent; }

        if (node.nodeType !== Node.ELEMENT_NODE) { return "";}

        var element:Element =<any> node;

        var style = getComputedStyle(element);

        if (style && style.display === "none") { return ""; }

        if (this.isTooltipElement(element)) { return ""; }

        var text = "";
        for (var i = 0; i < node.childNodes.length; i++) {
            text += this.getVisibleTextFromTree(node.childNodes[i]);
        }
        return text;
    }

    private handleKey = (element: HTMLElement) => {
        ko.dataFor(element).EquationText(this.getEditorRawText(element));

        dotvvm.postBack(
            "root",
            element,
            ["$parent.ReactionDetail", "$parent"],
            "QmlvY2hlbWljYWxSZWFjdGlvbkRldGFpbC5VcGRhdGVFcXVhdGlvbkFzeW5jKCk=",
            "", null,
            ["suppressOnUpdating"])
            .then(() => {
                if (element.isContentEditable) {
                    var styleSpans: StyleSpan[] = this.getStyleSpans(element);
                    var editorText: string = this.getEditorRawText(element);

                    var lastCaret = CarretHelper.getCaretPosition(element);
                    element.innerHTML = this.presenter.CreateRichText(editorText, styleSpans);
                    this.setCaret(element, lastCaret);

                    $('[data-toggle="tooltip"]').tooltip();
                }
            }).catch(r => {
                //We dont care, maybe it goes well next time
            });
    }


    private isTooltipElement(element: Element) {
        return element.classList.contains("tooltip-inner");
    }

    private getEditorRawText(element: HTMLElement) {
        var value: string = this.getVisibleTextFromTree(element);
        value = value == null
            ? ""
            : value;
        return value;
    }

    private getStyleSpans(element: HTMLElement) {
        var spans: StyleSpan[] = ko.toJS(ko.dataFor(element).EquationSpans());
        spans = spans == null
            ? []
            : spans;
        return spans;
    }
    //public addKnockoutHandlers = () => {
    //    ko.bindingHandlers.htmlLazy = {
    //        update: (element, valueAccessor) => {
    //        }
    //    };
    //    ko.bindingHandlers.contentEditable = {
    //        init: (element, valueAccessor, allBindingsAccessor) => {
    //            var value = ko.unwrap(valueAccessor()),
    //                htmlLazy = allBindingsAccessor().htmlLazy;

    //            $(element).on("input", function () {
    //                if (this.isContentEditable && ko.isWriteableObservable(htmlLazy)) {
    //                    htmlLazy(this.innerText);
    //                }
    //            }).on('change', function (e) {
    //            });
    //        },
    //        update: function (element, valueAccessor) {
    //            var value = ko.unwrap(valueAccessor());

    //            element.contentEditable = value;

    //            if (!element.isContentEditable) {
    //                $(element).trigger("input");
    //            }
    //        }
    //    };
    //}
}