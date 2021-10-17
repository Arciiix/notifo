﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace Notifo.Domain.Integrations.Telegram
{
    public sealed class TelegramBotClientPool
    {
        private readonly IMemoryCache memoryCache;

        public TelegramBotClientPool(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public ITelegramBotClient GetBotClient(string accessToken)
        {
            var cacheKey = $"TelegramBotClient_{accessToken}";

            var found = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var botClient = new TelegramBotClient(accessToken);

                return botClient;
            });

            return found;
        }
    }
}
