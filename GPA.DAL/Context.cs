using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.DAL
{
    public class Context
    {
        static void Main(string[] args) { }

        private Context() { }

        private static Context _instance;

        public static Context Current
        {
            get
            {
                
                if (_instance == null)
                    _instance = new Context();
                return _instance;
            }
        }

        private static Entities dALEntities;

        public Entities AccessContext
        {
            get
            {

                if (dALEntities == null)
                    dALEntities = new Entities();
                return dALEntities;
            }
        }
    }

    public class SharedObjectContext
    {
        private readonly Entities context;

        #region Singleton Pattern

        // Static members are lazily initialized.
        // .NET guarantees thread safety for static initialization.
        private static readonly SharedObjectContext instance = new SharedObjectContext();

        // Make the constructor private to hide it. 
        // This class adheres to the singleton pattern.
        private SharedObjectContext()
        {
            // Create the ObjectContext.
            context = new Entities();
        }

        // Return the single instance of the ClientSessionManager type.
        public static SharedObjectContext Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public Entities Context
        {
            get
            {
                return context;
            }
        }
    }
}
