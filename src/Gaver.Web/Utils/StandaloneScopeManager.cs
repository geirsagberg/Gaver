using LightInject;

namespace Gaver.Web.Utils
{
    internal class StandaloneScopeManager : IScopeManager
    {
        private readonly LogicalThreadStorage<Scope> currentScope = new LogicalThreadStorage<Scope>();

        public StandaloneScopeManager(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }

        public Scope CurrentScope
        {
            get { return currentScope.Value; }
            set { currentScope.Value = value; }
        }

        public IServiceFactory ServiceFactory { get; }

        public Scope BeginScope()
        {
            var scope = new Scope(this, null);
            currentScope.Value = scope;
            return scope;
        }

        public void EndScope(Scope scope)
        {
            currentScope.Value = null;
        }
    }

    public class StandaloneScopeManagerProvider : ScopeManagerProvider
    {
        protected override IScopeManager CreateScopeManager(IServiceFactory serviceFactory)
        {
            return new StandaloneScopeManager(serviceFactory);
        }
    }
}