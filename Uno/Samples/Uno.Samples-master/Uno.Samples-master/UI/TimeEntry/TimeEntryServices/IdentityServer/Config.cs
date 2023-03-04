﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("TimeEntryApi", "Time Entry API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                new Client
                {
                    ClientId = "TimeEntryUno",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("A2W7aQVFQWRX".Sha256())
                    },
                    AllowedScopes = { "TimeEntryApi" }
                }
            };
    }
}