using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;
using C0.Utils;

namespace C0.SymbolTable
{
    public class SymbolTable
    {
        private static SymbolTable _symbol;
        private readonly Dictionary<string, string> _parent;
        private readonly Dictionary<string, TokenType> _funcs;
        private readonly Dictionary<string, int> _offset;
        private readonly Dictionary<string, int> _level;
        private readonly Dictionary<Tuple<string, string>, int> _constVars;
        private readonly Dictionary<Tuple<string, string>, int> _initializedVars;
        private readonly Dictionary<Tuple<string, string>, int> _uninitializedVars;
        private readonly Dictionary<Tuple<string, string>, TokenType> _varType;

        private readonly Dictionary<string, List<Tuple<TokenType, string>>> _functions;
        private readonly Dictionary<string, int> _functionsIndex;

        private SymbolTable()
        {
            _parent = new Dictionary<string, string>();
            _constVars = new Dictionary<Tuple<string, string>, int>();
            _initializedVars = new Dictionary<Tuple<string, string>, int>();
            _uninitializedVars = new Dictionary<Tuple<string, string>, int>();
            _functions = new Dictionary<string, List<Tuple<TokenType, string>>>();
            _funcs = new Dictionary<string, TokenType>();
            _offset = new Dictionary<string, int>();
            _level = new Dictionary<string, int>();
            _functionsIndex = new Dictionary<string, int>();
            _varType = new Dictionary<Tuple<string, string>, TokenType>();
        }

        public static SymbolTable GetInstance()
        {
            return _symbol ?? (_symbol = new SymbolTable());
        }

        public bool IsDeclaredCurDomain(string s, string id)
        {
            return _uninitializedVars.ContainsKey(new Tuple<string, string>(s, id)) ||
                   _constVars.ContainsKey(new Tuple<string, string>(s, id)) ||
                   _initializedVars.ContainsKey(new Tuple<string, string>(s, id));
        }
        public bool IsFuncCurDomain(string id)
        {
            return _functions.ContainsKey(id);
        }
        public bool IsDeclaredAllDomain(string s, string id)
        {
            s = FindNearestPar(id, s);
            if (s == null)
            {
                return false;
            }

            return true;
        }

        public bool IsUninitializedVariable(string s, string id)
        {
            s = FindNearestPar(id, s);
            if (s == null)
            {
                return false;
            }
            return _uninitializedVars.ContainsKey(new Tuple<string, string>(s, id));
        }

        public bool IsInitializedVariable(string s, string id)
        {
            s = FindNearestPar(id, s);
            if (s == null)
            {
                return false;
            }
            return _initializedVars.ContainsKey(new Tuple<string, string>(s, id));
        }

        public bool IsConstVariable(string s, string id)
        {
            s = FindNearestPar(id, s);
            if (s == null)
            {
                return false;
            }
            return _constVars.ContainsKey(new Tuple<string, string>(s, id));
        }

        public bool IsFunciton(string id)
        {
            return _funcs.ContainsKey(id);
        }

        public void AddUninitializedVariable(string s, string id, TokenType type)
        {
            _uninitializedVars[new Tuple<string, string>(s, id)] = 0;
            _varType[new Tuple<string, string>(s, id)] = type;
        }

        public void AddInitializedVariable(string s, string id, TokenType type)
        {
            _initializedVars[new Tuple<string, string>(s, id)] = 0;
            _varType[new Tuple<string, string>(s, id)] = type;
        }

        public void AddConstVariable(string s, string id, TokenType type)
        {
            _constVars[new Tuple<string, string>(s, id)] = 0;
            _varType[new Tuple<string, string>(s, id)] = type;
        }
        public void AddFunctionParm(string id, string name, TokenType type)
        {
            if (!_functions.ContainsKey(id))
            {
                _functions[id] = new List<Tuple<TokenType, string>>();
            }
            _functions[id].Add(new Tuple<TokenType, string>(type, name));
        }
        public TokenType GeFuncType(string name)
        {
            return _funcs[name];
        }
        public List<Tuple<TokenType, string>> GetParams(string name)
        {
            if (!_functions.ContainsKey(name))
            {
                _functions[name] = new List<Tuple<TokenType, string>>();
            }
            return _functions[name];
        }
        public void AddFuncName(TokenType type, string name)
        {
            _functionsIndex[name] = _functionsIndex.Count - 1;
            _funcs[name] = type;
            _level[name] = 0;
            _offset[name] = 0;
        }
        public void AddParent(string parent, string name)
        {
            _parent[name] = parent;
            _level[name] = _level[parent] + 1;
        }
        public string RandomFuncName()
        {
            return System.Guid.NewGuid().ToString();
        }

        public void UpdateUninitializedOffset(string id, string par)
        {
            _uninitializedVars[new Tuple<string, string>(par, id)] = _offset[par];
            _offset[par] += 1;
        }
        public void UpdateInitializedOffset(string id, string par)
        {
            _initializedVars[new Tuple<string, string>(par, id)] = _offset[par];
            _offset[par] += 1;
        }
        public void UpdateConstOffset(string id, string par)
        {
            _constVars[new Tuple<string, string>(par, id)] = _offset[par];
            _offset[par] += 1;
        }

        public void InitializeVar(string id, string par)
        {
            var key = new Tuple<string, string>(FindNearestPar(id, par), id);
            _initializedVars[key] = _uninitializedVars[key];
            _uninitializedVars.Remove(key);
        }

        public Tuple<int, int> GetLevelOffset(string id, string par)
        {

            string np = FindNearestPar(id, par);
            if (np == null)
            {
                return null;
            }
            var key = new Tuple<string, string>(np, id);
            par = np;
            if (_uninitializedVars.ContainsKey(key))
            {
                return new Tuple<int, int>(_level[par], _uninitializedVars[key]);
            }

            if (_initializedVars.ContainsKey(key))
            {
                return new Tuple<int, int>(_level[par], _initializedVars[key]);
            }

            if (_constVars.ContainsKey(key))
            {
                return new Tuple<int, int>(_level[par], _constVars[key]);
            }

            return null;
        }

        public string FindNearestPar(string id, string par)
        {
            while (_parent.ContainsKey(par))
            {
                var key = new Tuple<string, string>(par, id);
                if (_uninitializedVars.ContainsKey(key))
                {
                    return par;
                }

                if (_initializedVars.ContainsKey(key))
                {
                    return par;
                }

                if (_constVars.ContainsKey(key))
                {
                    return par;
                }
                par = _parent[par];
            }
            Tuple<string, string> kk = new Tuple<string, string>(par, id);
            if (_uninitializedVars.ContainsKey(kk))
            {
                return par;
            }

            if (_initializedVars.ContainsKey(kk))
            {
                return par;
            }

            if (_constVars.ContainsKey(kk))
            {
                return par;
            }
            return null;
        }

        public int GetFuncIndex(string name)
        {
            return _functionsIndex[name];
        }
        public int GetFuncLevel(string name)
        {
            return _level[name];
        }

        public TokenType GetIdType(string par, string id)
        {
            var key = new Tuple<string, string>(FindNearestPar(id, par), id);
            return _varType[key];
        }
    }

}
