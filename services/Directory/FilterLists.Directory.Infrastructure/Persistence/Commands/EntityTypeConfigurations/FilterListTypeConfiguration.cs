﻿using System.Globalization;
using EFCore.NamingConventions.Internal;
using FilterLists.Directory.Infrastructure.Persistence.Queries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FilterList = FilterLists.Directory.Domain.Aggregates.FilterLists.FilterList;

namespace FilterLists.Directory.Infrastructure.Persistence.Commands.EntityTypeConfigurations;

internal class FilterListTypeConfiguration : IEntityTypeConfiguration<FilterList>
{
    public virtual void Configure(EntityTypeBuilder<FilterList> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.HasMany(f => f.Syntaxes)
            .WithMany(s => s.FilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(FilterListSyntax))}es");
                e.Property<long>(nameof(FilterListSyntax.FilterListId));
                e.Property<long>(nameof(FilterListSyntax.SyntaxId));
                e.HasKey(nameof(FilterListSyntax.FilterListId), nameof(FilterListSyntax.SyntaxId));
            });
        builder.HasMany(f => f.Languages)
            .WithMany(l => l.FilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(FilterListLanguage))}s");
                e.Property<long>(nameof(FilterListLanguage.FilterListId));
                e.Property<long>(nameof(FilterListLanguage.LanguageId));
                e.HasKey(nameof(FilterListLanguage.FilterListId), nameof(FilterListLanguage.LanguageId));
            });
        builder.HasMany(f => f.Tags)
            .WithMany(t => t.FilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(FilterListTag))}s");
                e.Property<long>(nameof(FilterListTag.FilterListId));
                e.Property<long>(nameof(FilterListTag.TagId));
                e.HasKey(nameof(FilterListTag.FilterListId), nameof(FilterListTag.TagId));
            });
        builder.HasMany(f => f.Maintainers)
            .WithMany(m => m.FilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(FilterListMaintainer))}s");
                e.Property<long>(nameof(FilterListMaintainer.FilterListId));
                e.Property<long>(nameof(FilterListMaintainer.MaintainerId));
                e.HasKey(nameof(FilterListMaintainer.FilterListId), nameof(FilterListMaintainer.MaintainerId));
            });
        builder.HasMany(f => f.UpstreamFilterLists)
            .WithMany(f => f.ForkFilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(Fork))}s");
                e.Property<long>(nameof(Fork.UpstreamFilterListId));
                e.Property<long>(nameof(Fork.ForkFilterListId));
                e.HasKey(nameof(Fork.UpstreamFilterListId), nameof(Fork.ForkFilterListId));
            });
        builder.HasMany(f => f.IncludedInFilterLists)
            .WithMany(f => f.IncludesFilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(Merge))}s");
                e.Property<long>(nameof(Merge.IncludedInFilterListId));
                e.Property<long>(nameof(Merge.IncludesFilterListId));
                e.HasKey(nameof(Merge.IncludedInFilterListId), nameof(Merge.IncludesFilterListId));
            });
        builder.HasMany(f => f.DependencyFilterLists)
            .WithMany(f => f.DependentFilterLists)
            .UsingEntity(e =>
            {
                e.ToTable($"{nr.RewriteName(nameof(Dependent))}s");
                e.Property<long>(nameof(Dependent.DependencyFilterListId));
                e.Property<long>(nameof(Dependent.DependentFilterListId));
                e.HasKey(nameof(Dependent.DependencyFilterListId), nameof(Dependent.DependentFilterListId));
            });
        builder.HasMany(f => f.Changes)
            .WithOne()
            .HasForeignKey(nameof(Change.FilterListId));
        builder.Navigation(f => f.Changes)
            .AutoInclude();
        builder.Navigation(f => f.ViewUrls)
            .AutoInclude();
    }
}
