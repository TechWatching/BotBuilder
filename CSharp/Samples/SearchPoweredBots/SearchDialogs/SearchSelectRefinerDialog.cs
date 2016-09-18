﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Bot Framework: http://botframework.com
// 
// Bot Builder SDK Github:
// https://github.com/Microsoft/BotBuilder
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.Bot.Sample.SearchDialogs
{
    [Serializable]
    public class SearchSelectRefinerDialog : IDialog<string>
    {
        protected readonly SearchQueryBuilder queryBuilder;
        protected readonly IEnumerable<string> refiners;
        protected readonly PromptStyler promptStyler;

        public SearchSelectRefinerDialog(IEnumerable<string> refiners, SearchQueryBuilder queryBuilder = null, PromptStyler promptStyler = null)
        {
            if (refiners == null)
            {
                throw new ArgumentNullException("refiners");
            }

            this.refiners = refiners;
            this.queryBuilder = queryBuilder ?? new SearchQueryBuilder();
            this.promptStyler = promptStyler;
        }

        public Task StartAsync(IDialogContext context)
        {
            IEnumerable<string> unusedRefiners = this.refiners;
            if (this.queryBuilder != null)
            {
                unusedRefiners = unusedRefiners.Except(this.queryBuilder.Refinements.Keys, StringComparer.OrdinalIgnoreCase);
            }

            PromptOptions<string> promptOptions = new PromptOptions<string>("What do you want to refine by?", options: unusedRefiners.ToList(), promptStyler: this.promptStyler);
            PromptDialog.Choice(context, ReturnSelection, promptOptions);
            return Task.CompletedTask;
        }

        protected virtual async Task ReturnSelection(IDialogContext context, IAwaitable<string> input)
        {
            context.Done(await input);
        }
    }
}
