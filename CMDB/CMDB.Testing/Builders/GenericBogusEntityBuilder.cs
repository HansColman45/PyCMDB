using Bogus;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CMDB.Testing.Builders
{
    public class GenericBogusEntityBuilder<T> where T : class
    {
        private readonly Faker _faker = new();

        protected Faker<T> EntityFaker { get; }

        public GenericBogusEntityBuilder()
        {
            EntityFaker = new Faker<T>();
        }

        public GenericBogusEntityBuilder<T> SetDefaultRules(Action<Faker, T> rules)
        {
            EntityFaker.Rules(rules);
            return this;
        }

        public GenericBogusEntityBuilder<T> With<TProp>(Expression<Func<T, TProp>> expression, Func<TProp> value)
        {
            EntityFaker.RuleFor(expression, value);
            return this;
        }

        public GenericBogusEntityBuilder<T> With<TProp>(Expression<Func<T, TProp>> expression, TProp value)
        {
            EntityFaker.RuleFor(expression, value);
            return this;
        }

        public GenericBogusEntityBuilder<T> With<TProp>(Expression<Func<T, TProp>> expression, Func<Faker, TProp> faker)
        {
            EntityFaker.RuleFor(expression, faker(_faker));
            return this;
        }

        public virtual T Build()
        {
            return EntityFaker.Generate();
        }

        public virtual IList<T> BuildList(int count)
        {
            return EntityFaker.Generate(count);
        }
    }
}
