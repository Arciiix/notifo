﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain.Integrations.Resources;

namespace Notifo.Domain.Integrations.Http;

public sealed partial class HttpIntegration : IIntegration
{
    private readonly IHttpClientFactory httpClientFactory;

    private static readonly IntegrationProperty HttpUrlProperty = new IntegrationProperty("Url", PropertyType.Text)
    {
        EditorLabel = Texts.Webhook_URLLabel,
        EditorDescription = Texts.Webhook_URLHints,
        IsRequired = true,
        Summary = true
    };

    private static readonly IntegrationProperty HttpMethodProperty = new IntegrationProperty("Method", PropertyType.Text)
    {
        EditorLabel = Texts.Webhook_MethodLabel,
        EditorDescription = Texts.Webhook_MethodHints,
        AllowedValues = ["POST", "GET", "PATCH", "PUT", "DELETE"],
        IsRequired = false,
    };

    private static readonly IntegrationProperty SendAlwaysProperty = new IntegrationProperty("SendAlways", PropertyType.Boolean)
    {
        EditorLabel = Texts.Webhook_SendAlwaysLabel,
        EditorDescription = Texts.Webhook_SendAlwaysHints,
        IsRequired = false,
        Summary = false
    };

    private static readonly IntegrationProperty SendConfirmProperty = new IntegrationProperty("SendConfirm", PropertyType.Boolean)
    {
        EditorLabel = Texts.Webhook_SendConfirmLabel,
        EditorDescription = Texts.Webhook_SendConfirmHints,
        IsRequired = false,
        Summary = false
    };

    public IntegrationDefinition Definition { get; } =
        new IntegrationDefinition(
            "Webhook",
            "Webhook",
            "<svg width='2500' height='2334' viewBox='0 0 256 239' xmlns='http://www.w3.org/2000/svg' preserveAspectRatio='xMidYMid'><path d='M119.54 100.503c-10.61 17.836-20.775 35.108-31.152 52.25-2.665 4.401-3.984 7.986-1.855 13.58 5.878 15.454-2.414 30.493-17.998 34.575-14.697 3.851-29.016-5.808-31.932-21.543-2.584-13.927 8.224-27.58 23.58-29.757 1.286-.184 2.6-.205 4.762-.367l23.358-39.168C73.612 95.465 64.868 78.39 66.803 57.23c1.368-14.957 7.25-27.883 18-38.477 20.59-20.288 52.002-23.573 76.246-8.001 23.284 14.958 33.948 44.094 24.858 69.031-6.854-1.858-13.756-3.732-21.343-5.79 2.854-13.865.743-26.315-8.608-36.981-6.178-7.042-14.106-10.733-23.12-12.093-18.072-2.73-35.815 8.88-41.08 26.618-5.976 20.13 3.069 36.575 27.784 48.967z' fill='#C73A63'/><path d='M149.841 79.41c7.475 13.187 15.065 26.573 22.587 39.836 38.02-11.763 66.686 9.284 76.97 31.817 12.422 27.219 3.93 59.457-20.465 76.25-25.04 17.238-56.707 14.293-78.892-7.851 5.654-4.733 11.336-9.487 17.407-14.566 21.912 14.192 41.077 13.524 55.305-3.282 12.133-14.337 11.87-35.714-.615-49.75-14.408-16.197-33.707-16.691-57.035-1.143-9.677-17.168-19.522-34.199-28.893-51.491-3.16-5.828-6.648-9.21-13.77-10.443-11.893-2.062-19.571-12.275-20.032-23.717-.453-11.316 6.214-21.545 16.634-25.53 10.322-3.949 22.435-.762 29.378 8.014 5.674 7.17 7.477 15.24 4.491 24.083-.83 2.466-1.905 4.852-3.07 7.774z' fill='#4B4B4B'/><path d='M167.707 187.21h-45.77c-4.387 18.044-13.863 32.612-30.19 41.876-12.693 7.2-26.373 9.641-40.933 7.29-26.808-4.323-48.728-28.456-50.658-55.63-2.184-30.784 18.975-58.147 47.178-64.293 1.947 7.071 3.915 14.21 5.862 21.264-25.876 13.202-34.832 29.836-27.59 50.636 6.375 18.304 24.484 28.337 44.147 24.457 20.08-3.962 30.204-20.65 28.968-47.432 19.036 0 38.088-.197 57.126.097 7.434.117 13.173-.654 18.773-7.208 9.22-10.784 26.191-9.811 36.121.374 10.148 10.409 9.662 27.157-1.077 37.127-10.361 9.62-26.73 9.106-36.424-1.26-1.992-2.136-3.562-4.673-5.533-7.298z' fill='#4A4A4A'/></svg>",
            [
                HttpUrlProperty,
                HttpMethodProperty,
                SendConfirmProperty
            ],
            [],
            new HashSet<string>
            {
                    Providers.Webhook
            })
        {
            Description = Texts.Webhook_Description
        };

    public HttpIntegration(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }
}
