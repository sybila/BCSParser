/// <reference path="lib/knockout/knockout.d.ts" />
/// <reference path="lib/jquery/jquery.d.ts" />

class CodeEditor {
    private lastCaret = 0;

    public getCharacterOffsetWithin = (range, node) => {

        var filterFunction =
            node => {
                var nodeRange = document.createRange();
                nodeRange.selectNode(node);
                return nodeRange.compareBoundaryPoints(Range.END_TO_END, range) < 1 ?
                    NodeFilter.FILTER_ACCEPT : NodeFilter.FILTER_REJECT;
            }

        var treeWalker = document.createTreeWalker(
            node,
            NodeFilter.SHOW_TEXT,
            { acceptNode: filterFunction },
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

    public getCaretPosition = (node) => {
        var caretPos = 0, sel, range;

        const doc = document as any;

        if (window.getSelection) {
            sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0);
                return this.getCharacterOffsetWithin(range, node);
            }
        } else if (doc.selection && doc.createRange) {
            range = doc.selection.createRange();
            return this.getCharacterOffsetWithin(range, node);
        }
        return caretPos;
    }

    public setCaret = (e, cp) => {
        var carretSet = false;

        var placeCaret = function (item, position) {
            var range = document.createRange();
            range.setStart(item, position)
            range.setEnd(item, position)

            var sel = window.getSelection()
            sel.removeAllRanges()
            sel.addRange(range)
            carretSet = true;
        }

        var setCaretCore = (element, caretPosition) => {
            var charsInNode = 0;

            element.childNodes.forEach(function (item, index) {
                if (carretSet) { return 0; }

                if (item.nodeType === 3) {
                    var textContentLenght = item.textContent.length;
                    var relativeCaretPosition = caretPosition - charsInNode;

                    if (0 <= relativeCaretPosition && relativeCaretPosition <= textContentLenght) {
                        placeCaret(item, relativeCaretPosition);
                    }
                    else {
                        charsInNode += textContentLenght
                    }
                    return;
                }
                if (item.nodeType == 1) {
                    charsInNode += setCaretCore(item, caretPosition - charsInNode)
                }
            });

            if (carretSet) { return 0; }
            return charsInNode;
        }

        setCaretCore(e, cp);
    }

    public addKnockoutHandlers = () => {
        ko.bindingHandlers.htmlLazy = {
            update: (element, valueAccessor) => {
                var value = ko.unwrap(valueAccessor());

                if (element.isContentEditable) {
                    this.lastCaret = this.getCaretPosition(element)
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