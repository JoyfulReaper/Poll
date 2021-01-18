// https://gist.github.com/mr5z/7a72471eef039093c0046a2915f0ccda
// Unknown license, Permission for usage granted by author mr5z

using System;
using System.Collections.Generic;
using ParameterizedFunc = System.Func<object, object>;

namespace PollApi.Helpers
{
	public class Mapper
	{
		private readonly IDictionary<(Type, Type), ParameterizedFunc> objectDictionary =
			new Dictionary<(Type, Type), ParameterizedFunc>();

		private static Lazy<Mapper> _instance = new Lazy<Mapper>(() => new Mapper());
		public static Mapper Instance => _instance.Value;

		private Mapper()
		{
		}

		public void Register<TSource, TDestination>(Func<TSource, TDestination> mapping)
			where TSource : class
			where TDestination : class
		{
			var key = ToKey<TSource, TDestination>();
			objectDictionary[key] = (arg) => mapping((TSource)arg);
		}

		public TDestination Map<TSource, TDestination>(TSource source)
			where TSource : class
			where TDestination : class
		{
			var key = ToKey<TSource, TDestination>();
			return (TDestination)objectDictionary[key](source);
		}

		private (Type, Type) ToKey<TSource, TDestination>()
		{
			return (typeof(TSource), typeof(TDestination));
		}
	}
}
