/*
 * Notifo.io
 *
 * @license
 * Copyright (c) Sebastian Stehle. All rights reserved.
 */

/* eslint-disable quote-props */

import * as CodeMirror from 'codemirror';
import { useDebounce, useSimpleQuery } from '@app/framework';
import { Clients, EmailPreviewDto, EmailPreviewType } from '@app/service';

type MarkupResponse = { rendering: EmailPreviewDto; template?: string };

const DEFAULT_RESPONSE: MarkupResponse = { rendering: {} };

export function usePreview(appId: string, sourceTemplate: string, type: EmailPreviewType): MarkupResponse {
    const template = useDebounce(sourceTemplate, 500);

    const query = useSimpleQuery<MarkupResponse>({
        queryKey: [template, type],
        queryFn: async () => {
            if (!template) {
                return { rendering: {} };
            }

            try {
                const rendering = await Clients.EmailTemplates.postPreview(appId, { template, type });

                return { rendering, template };
            } catch (ex: any) {
                const rendering: EmailPreviewDto = {
                    errors: [{
                        message: ex.message,
                    } as any],
                };
                
                return { rendering, template };
            }
        },
        defaultValue: DEFAULT_RESPONSE,
    });
    
    return query.value;
}

export function completeAfter(editor: CodeMirror.Editor, predicate?: (cursor: CodeMirror.Position) => boolean) {
    const cursor = editor.getCursor();

    if (!predicate || predicate(cursor)) {
        setTimeout(() => {
            if (!editor.state.completionActive) {
                const options: any = {
                    completeSingle: false,
                };

                editor.showHint(options);
            }
        }, 100);
    }

    return CodeMirror.Pass;
}

export function completeIfAfterLt(cm: CodeMirror.Editor) {
    return completeAfter(cm, (cursor) => {
        const current = cm.getRange(CodeMirror.Pos(cursor.line, cursor.ch - 1), cursor);

        return current === '<';
    });
}

export function completeIfInTag(cm: CodeMirror.Editor) {
    return completeAfter(cm, (cursor) => {
        const token = cm.getTokenAt(cursor);

        if (token.type === 'string' && (!/['"]/.test(token.string.charAt(token.string.length - 1)) || token.string.length === 1)) {
            return false;
        }

        const inner = CodeMirror.innerMode(cm.getMode(), token.state).state;

        return inner.tagName;
    });
}
