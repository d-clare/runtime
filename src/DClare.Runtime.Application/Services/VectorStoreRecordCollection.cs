// Copyright © 2025-Present The DClare Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Extensions.VectorData;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IVectorStoreRecordCollection"/> interface.
/// </summary>
/// <typeparam name="TKey">The type of manage records key.</typeparam>
/// <param name="underlyingCollection">The underlying <see cref="IVectorStoreRecordCollection{TKey, TRecord}"/>.</param>
public class VectorStoreRecordCollection<TKey>(IVectorStoreRecordCollection<TKey, TextEmbeddingRecord<TKey>> underlyingCollection)
    : IVectorStoreRecordCollection
    where TKey : notnull
{

    /// <summary>
    /// Gets the underlying <see cref="IVectorStoreRecordCollection{TKey, TRecord}"/>.
    /// </summary>
    protected IVectorStoreRecordCollection<TKey, TextEmbeddingRecord<TKey>> UnderlyingCollection { get; } = underlyingCollection;

    /// <inheritdoc/>
    public virtual string Name => UnderlyingCollection.Name;

    /// <inheritdoc/>
    public virtual Task<bool> CollectionExistsAsync(CancellationToken cancellationToken = default) => UnderlyingCollection.CollectionExistsAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual Task CreateCollectionAsync(CancellationToken cancellationToken = default) => UnderlyingCollection.CreateCollectionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual Task CreateCollectionIfNotExistsAsync(CancellationToken cancellationToken = default) => UnderlyingCollection.CreateCollectionIfNotExistsAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual Task DeleteAsync(object key, CancellationToken cancellationToken = default) => UnderlyingCollection.DeleteAsync((TKey)key, cancellationToken);

    /// <inheritdoc/>
    public virtual Task DeleteAsync(IEnumerable<object> keys, CancellationToken cancellationToken = default) => UnderlyingCollection.DeleteAsync(keys.Cast<TKey>(), cancellationToken);

    /// <inheritdoc/>
    public virtual Task DeleteCollectionAsync(CancellationToken cancellationToken = default) => UnderlyingCollection.DeleteCollectionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<TextEmbeddingRecord?> GetAsync(object key, GetRecordOptions? options = null, CancellationToken cancellationToken = default) => await UnderlyingCollection.GetAsync((TKey)key, options, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TextEmbeddingRecord> GetAsync(IEnumerable<object> keys, GetRecordOptions? options = null, CancellationToken cancellationToken = default) => UnderlyingCollection.GetAsync(keys.Cast<TKey>(), options, cancellationToken);

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TextEmbeddingRecord> GetAsync(Expression<Func<TextEmbeddingRecord, bool>> filter, int top, GetFilteredRecordOptions<TextEmbeddingRecord>? options = null, CancellationToken cancellationToken = default) => UnderlyingCollection.GetAsync(record => filter.Compile()(record), top, new()
    {
        IncludeVectors = options?.IncludeVectors ?? false,
        Skip = options?.Skip ?? 0
    }, cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<object> UpsertAsync(TextEmbeddingRecord record, CancellationToken cancellationToken = default) => await UnderlyingCollection.UpsertAsync(ConvertToKeyedRecord(record), cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<IReadOnlyList<object>> UpsertAsync(IEnumerable<TextEmbeddingRecord> records, CancellationToken cancellationToken = default) => [.. await UnderlyingCollection.UpsertAsync(records.Select(ConvertToKeyedRecord), cancellationToken).ConfigureAwait(false)];

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<VectorSearchResult<SemanticSearchResult>> SearchAsync<TInput>(TInput value, int top, VectorSearchOptions<SemanticSearchResult>? options = null, CancellationToken cancellationToken = default)
        where TInput : notnull
    {
        return UnderlyingCollection.SearchAsync(value, top, new()
        {
            //Filter = options.Filter, //todo: convert
            IncludeVectors = options?.IncludeVectors ?? false,
            Skip = options?.Skip ?? 0,
            //VectorProperty = options.VectorProperty //todo: convert
        }, cancellationToken).Select(r => new VectorSearchResult<SemanticSearchResult>(r.Record.AsSearchResult(), r.Score));
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<VectorSearchResult<SemanticSearchResult>> SearchEmbeddingAsync<TVector>(TVector vector, int top, VectorSearchOptions<SemanticSearchResult>? options = null, CancellationToken cancellationToken = default)
        where TVector : notnull
    {
        return UnderlyingCollection.SearchEmbeddingAsync(vector, top, new()
        {
            //Filter = options.Filter, //todo: convert
            IncludeVectors = options?.IncludeVectors ?? false,
            Skip = options?.Skip ?? 0,
            //VectorProperty = options.VectorProperty //todo: convert
        }, cancellationToken).Select(r => new VectorSearchResult<SemanticSearchResult>(r.Record.AsSearchResult(), r.Score));
    }

    /// <summary>
    /// Converts the specified <see cref="TextEmbeddingRecord"/> into a new <see cref="TextEmbeddingRecord{TKey}"/>.
    /// </summary>
    /// <param name="record">The <see cref="TextEmbeddingRecord"/> to convert.</param>
    /// <returns>A new <see cref="TextEmbeddingRecord{TKey}"/>.</returns>
    protected virtual TextEmbeddingRecord<TKey> ConvertToKeyedRecord(TextEmbeddingRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        var keyType = typeof(TKey);
        var key = keyType == typeof(Guid) ? Guid.NewGuid() : (object)Guid.NewGuid().ToString("N");
        return new()
        {
            Key = (TKey)key,
            CreatedAt = record.CreatedAt,
            Text = record.Text,
            Reference = record.Reference,
            Metadata = record.Metadata,
            Tags = record.Tags,
            TextEmbedding = record.TextEmbedding
        };
    }

}
