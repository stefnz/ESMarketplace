﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using static Marketplace.ClassifiedAds.ReadModels;
using static Marketplace.Domain.ClassifiedAd;

namespace Marketplace.ClassifiedAds {
    public static class Queries {
        public static Task<List<PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetPublishedClassifiedAds query)
            => session.Query<Domain.ClassifiedAd>()
                .Where(x => x.State == ClassifiedAdState.Active)
                .Select(x =>
                    new PublicClassifiedAdListItem {
                        ClassifiedAdId = x.Id.Value,
                        Price = x.Price.Amount,
                        Title = x.Title.Value,
                        CurrencyCode = x.Price.Currency.CurrencyCode
                    }
                )
                .PagedList(query.Page, query.PageSize);

        public static Task<List<PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetOwnersClassifiedAd query)
            => session.Query<Domain.ClassifiedAd>()
                .Where(x => x.OwnerId.Value == query.OwnerId)
                .Select(
                    x =>
                        new PublicClassifiedAdListItem {
                            ClassifiedAdId = x.Id.Value,
                            Price = x.Price.Amount,
                            Title = x.Title.Value,
                            CurrencyCode = x.Price.Currency.CurrencyCode
                        }
                )
                .PagedList(query.Page, query.PageSize);

        public static Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session, QueryModels.GetPublicClassifiedAd query)
            => (from ad in session.Query<Domain.ClassifiedAd>()
                where ad.Id.Value == query.ClassifiedAdId
                let user = RavenQuery
                    .Load<Domain.UserProfiles.UserProfile>(
                        "UserProfile/" + ad.OwnerId.Value
                    )
                select new ClassifiedAdDetails {
                    ClassifiedAdId = ad.Id.Value,
                    Title = ad.Title.Value,
                    Description = ad.Text.Value,
                    Price = ad.Price.Amount,
                    CurrencyCode = ad.Price.Currency.CurrencyCode,
                    SellersDisplayName = user.DisplayName.Value
                }).SingleAsync();

        private static Task<List<T>> PagedList<T>(this IRavenQueryable<T> query, int page, int pageSize)
            => query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}