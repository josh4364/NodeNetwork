using System;
using System.Collections.Generic;

namespace NeoUnity.Neo4j
{
    public class RootObject
    {
        public List<Result> results { get; set; }
        public List<object> errors { get; set; }
    }
}
