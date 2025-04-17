﻿// Copyright © 2025-Present The DClare Authors
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

namespace DClare.Runtime.Application;

/// <summary>
/// Describes an OAUTH2 token
/// </summary>
[DataContract]
public record OAuth2Token
{

    /// <summary>
    /// Gets the UTC date and time at which the <see cref="OAuth2Token"/> has been created
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the OAUTH2 token type
    /// </summary>
    [DataMember(Order = 1, Name = "token_type"), JsonPropertyName("token_type"), JsonPropertyOrder(1), YamlMember(Alias = "token_type", Order = 1)]
    public virtual string? TokenType { get; set; }

    /// <summary>
    /// Gets the OAUTH2 token id
    /// </summary>
    [DataMember(Order = 2, Name = "token_id"), JsonPropertyName("token_id"), JsonPropertyOrder(2), YamlMember(Alias = "token_id", Order = 2)]
    public virtual string? TokenId { get; set; }

    /// <summary>
    /// Gets the OAUTH2 access token
    /// </summary>
    [DataMember(Order = 3, Name = "access_token"), JsonPropertyName("access_token"), JsonPropertyOrder(3), YamlMember(Alias = "access_token", Order = 3)]
    public virtual string? AccessToken { get; set; }

    /// <summary>
    /// Gets the OAUTH2 refresh token
    /// </summary>
    [DataMember(Order = 4, Name = "refresh_token"), JsonPropertyName("refresh_token"), JsonPropertyOrder(4), YamlMember(Alias = "refresh_token", Order = 4)]
    public virtual string? RefreshToken { get; set; }

    /// <summary>
    /// Gets the <see cref="OAuth2Token"/> Time To Live, in seconds
    /// </summary>
    [DataMember(Order = 5, Name = "expires_in"), JsonPropertyName("expires_in"), JsonPropertyOrder(5), YamlMember(Alias = "expires_in", Order = 5)]
    public virtual int Ttl { get; set; }

    /// <summary>
    /// Gets the UTC date and time at which the <see cref="OAuth2Token"/> expires
    /// </summary>
    [DataMember(Order = 6, Name = "expires_on"), JsonPropertyName("expires_on"), JsonPropertyOrder(6), YamlMember(Alias = "expires_on", Order = 6)]
    public virtual DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets a boolean indicating whether or not the <see cref="OAuth2Token"/> has expired
    /// </summary>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual bool HasExpired => this.ExpiresAt.HasValue ? DateTime.UtcNow > this.ExpiresAt : DateTime.UtcNow > this.CreatedAt.Add(TimeSpan.FromSeconds(this.Ttl));

}
