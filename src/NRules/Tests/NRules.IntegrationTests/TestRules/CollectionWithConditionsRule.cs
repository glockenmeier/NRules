﻿using System.Collections.Generic;
using System.Linq;
using NRules.IntegrationTests.TestAssets;

namespace NRules.IntegrationTests.TestRules
{
    public class CollectionWithConditionsRule : BaseRule
    {
        public int FactCount { get; set; }

        public override void Define()
        {
            IEnumerable<FactType1> collection1 = null;

            When()
                .Collect<FactType1>(() => collection1, f => f.TestProperty.StartsWith("Valid")).Where(x => x.Count() > 2);
            Then()
                .Do(ctx => Notifier.RuleActivated())
                .Do(ctx => SetCount(collection1.Count()));
        }

        private void SetCount(int count)
        {
            FactCount = count;
        }
    }
}