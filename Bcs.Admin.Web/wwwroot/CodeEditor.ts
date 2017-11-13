/// <reference path="lib/knockout/knockout.d.ts" />
/// <reference path="lib/jquery/jquery.d.ts" />

class CarretHelper
{
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
        if (range.startContainer.nodeType == 3) {
            charCount += range.startOffset;
        }
        return charCount;
    }
}

class CodeEditor {
    private lastCaret: number = 0;
    private carretSet: boolean = false;
    
    public setCaret = (e, cp) => {
        this.carretSet = false;
        this.setCaretCore(e, cp);
    }

    private setCaretCore = (element: Node, caretPosition: number) => {
        var charsInNode = 0;

        (element.childNodes  as any).forEach(
            (item:Node, index: number) => {
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
                else if (item.nodeType == 1)
                {
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

    public addKnockoutHandlers = () => {
        ko.bindingHandlers.htmlLazy = {
            update: (element, valueAccessor) => {
                var value = ko.unwrap(valueAccessor());

                if (element.isContentEditable) {
                    this.lastCaret = CarretHelper.getCaretPosition(element)
                    console.log(this.lastCaret);
                    element.innerHTML = value;
                    this.setCaret(element, this.lastCaret);
                }

            }
        };
        ko.bindingHandlers.contentEditable = {
            init: (element, valueAccessor, allBindingsAccessor) => {
                var value = ko.unwrap(valueAccessor()),
                    htmlLazy = allBindingsAccessor().htmlLazy;

                $(element).on("input", function () {
                    if (this.isContentEditable && ko.isWriteableObservable(htmlLazy)) {
                        htmlLazy(this.innerHTML);
                    }
                }).on('change', function (e) {
                });
            },
            update: function (element, valueAccessor) {
                var value = ko.unwrap(valueAccessor());

                element.contentEditable = value;

                if (!element.isContentEditable) {
                    $(element).trigger("input");
                }
            }
        };
    }
}