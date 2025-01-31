﻿using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

/// <summary>
/// A composite member translator that dispatches to multiple specialized member translators specific to Npgsql.
/// </summary>
public class NpgsqlMemberTranslatorProvider : RelationalMemberTranslatorProvider
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual NpgsqlJsonPocoTranslator JsonPocoTranslator { get; }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public NpgsqlMemberTranslatorProvider(
        RelationalMemberTranslatorProviderDependencies dependencies,
        IModel model,
        IRelationalTypeMappingSource typeMappingSource,
        INpgsqlSingletonOptions npgsqlSingletonOptions)
        : base(dependencies)
    {
        var sqlExpressionFactory = (NpgsqlSqlExpressionFactory)dependencies.SqlExpressionFactory;
        JsonPocoTranslator = new NpgsqlJsonPocoTranslator(typeMappingSource, sqlExpressionFactory, model);

        AddTranslators(
            new IMemberTranslator[] {
                new NpgsqlArrayTranslator(sqlExpressionFactory, JsonPocoTranslator, npgsqlSingletonOptions.UseRedshift),
                new NpgsqlBigIntegerMemberTranslator(sqlExpressionFactory),
                new NpgsqlDateTimeMemberTranslator(typeMappingSource, sqlExpressionFactory),
                new NpgsqlJsonDomTranslator(typeMappingSource, sqlExpressionFactory, model),
                new NpgsqlLTreeTranslator(typeMappingSource, sqlExpressionFactory, model),
                JsonPocoTranslator,
                new NpgsqlRangeTranslator(typeMappingSource, sqlExpressionFactory, model, npgsqlSingletonOptions),
                new NpgsqlStringMemberTranslator(sqlExpressionFactory),
                new NpgsqlTimeSpanMemberTranslator(sqlExpressionFactory),
            });
    }
}
