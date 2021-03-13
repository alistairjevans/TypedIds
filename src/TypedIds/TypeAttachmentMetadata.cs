#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;

namespace TypedIds
{
    public class TypeAttachmentMetadata
    {
        private ImmutableList<string> _attributeLiterals = ImmutableList<string>.Empty;
        private ImmutableHashSet<string> _additionalNamespaces = ImmutableHashSet<string>.Empty;

        public IReadOnlyCollection<string> AttributeLiterals => _attributeLiterals;

        public IReadOnlyCollection<string> AdditionalNamespaces => _additionalNamespaces;

        public void AddAttributeLiteral(string attribute)
        {
            _attributeLiterals = _attributeLiterals.Add(attribute);
        }

        public void AddNamespace(string additionalNamespace)
        {
            _additionalNamespaces = _additionalNamespaces.Add(additionalNamespace);
        }
    }
}
