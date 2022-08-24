﻿using System.Globalization;

namespace Marketplace.Domain;

public class ClassifiedAd {
    public ClassifiedAdId Id { get; }
    public UserId OwnerId { get; }
    public ClassifiedAdTitle Title { get; private set; }
    public ClassifiedAdText Text { get; private set; }
    public Price Price { get; private set; }
    public ClassifiedAdState State { get; private set; }
    public UserId ApprovedBy { get; private set; }


    public ClassifiedAd(ClassifiedAdId id, UserId ownerId) {
        Id = id;
        OwnerId = ownerId;
        State = ClassifiedAdState.Inactive;
        EnsureValidState();
    }

    public void SetTitle(ClassifiedAdTitle title) {
        Title = title;
        EnsureValidState();
    }

    public void UpdateText(ClassifiedAdText text) {
        Text = text;
        EnsureValidState();
    }

    public void UpdatePrice(Price price) {
        Price = price;
        EnsureValidState();
    }


    public void RequestPublish() {
        State = ClassifiedAdState.PendingReview;
        EnsureValidState();
    }

    public void EnsureValidState() {
        bool isValid =
            Id != null
            && OwnerId != null
            && (State switch {
                ClassifiedAdState.PendingReview =>
                    Title != null
                    && Text != null
                    && Price?.Amount > 0,
                ClassifiedAdState.Active =>
                    Title != null
                    && Text != null
                    && Price?.Amount > 0
                    && ApprovedBy != null,
                _ => true
            });

        if (!isValid) {
            // TODO: improve error capture and reporting
            throw new InvalidEntityStateException(this, $"Classied Ad validation failed in state {State}");
        }
    }
    public enum ClassifiedAdState {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold
    }
}