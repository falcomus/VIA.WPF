// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEntityMessages.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Record XEntityCreatedMessage ###
/// <summary>
/// Represents a message indicating that an entity was created.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="Entity">The created entity.</param>
public sealed record XEntityCreatedMessage<TEntity>(TEntity Entity);
#endregion

#region ### Record XEntityUpdatedMessage ###
/// <summary>
/// Represents a message indicating that an entity was updated.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="Entity">The updated entity.</param>
public sealed record XEntityUpdatedMessage<TEntity>(TEntity Entity);
#endregion

#region ### Record XEntityDeletedMessage ###
/// <summary>
/// Represents a message indicating that an entity was deleted.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="Entity">The deleted entity.</param>
public sealed record XEntityDeletedMessage<TEntity>(TEntity Entity);
#endregion

#region ### Record XEntitySavedMessage ###
/// <summary>
/// Represents a message indicating that an entity was saved.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="Entity">The saved entity.</param>
public sealed record XEntitySavedMessage<TEntity>(TEntity Entity);
#endregion

#region ### Record XRefreshRequestedMessage ###
/// <summary>
/// Represents a message indicating that a refresh was requested.
/// </summary>
/// <param name="Scope">The optional refresh scope.</param>
public sealed record XRefreshRequestedMessage(string? Scope = null);
#endregion
