using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Nrepo.Data;

namespace Nrepo.DataAccess.EntityFramework
{
	/// <summary>
	/// The base class for all the contexts.
	/// </summary>
	public abstract class EFDbContext<TEntity> : DbContext
		where TEntity : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		protected EFDbContext()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="model">The model that will back this context.</param>
		protected EFDbContext(DbCompiledModel model)
			: base(model)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
		public EFDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="objectContext">An existing ObjectContext to wrap with the new context.</param>
		/// <param name="dbContextOwnsObjectContext">If set to <c>true</c> the ObjectContext is disposed when the DbContext is disposed, otherwise the caller must dispose the connection.</param>
		public EFDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
			: base(objectContext, dbContextOwnsObjectContext)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
		/// <param name="model">The model that will back this context.</param>
		public EFDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="existingConnection">An existing connection to use for the new context.</param>
		/// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
		public EFDbContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFDbContext{TEntity}"/> class.
		/// </summary>
		/// <param name="existingConnection">An existing connection to use for the new context.</param>
		/// <param name="model">The model that will back this context.</param>
		/// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
		public EFDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
		{
		}

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		/// before the model has been locked down and used to initialize the context.  The default
		/// implementation of this method does nothing, but it can be overridden in a derived class
		/// such that the model can be further configured before it is locked down.
		/// </summary>
		/// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
		/// <remarks>
		/// Typically, this method is called only once when the first instance of a derived context
		/// is created.  The model for that context is then cached and is for all further instances of
		/// the context in the app domain.  This caching can be disabled by setting the ModelCaching
		/// property on the given ModelBuidler, but note that this can seriously degrade performance.
		/// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
		/// classes directly.
		/// </remarks>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			if (ReflectionHelper.IsInherited(typeof(IEditable), typeof(TEntity)))
			{
				modelBuilder.Entity<TEntity>().Ignore<bool>(ExpressionBuilder.GetPropertyExpression<TEntity, bool>(
					Expression.Parameter(typeof(TEntity)), ReflectionHelper.GetPropertyName<IEditable, bool>(e => e.IsUsed)));
			}
		}
	}
}
