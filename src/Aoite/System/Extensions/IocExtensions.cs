﻿using System;
using System.Collections.Generic;
using System.Linq;
using Aoite.DI;

/// <summary>
/// 表示 <see cref="IIocContainer"/> 的扩展方法。
/// </summary>
public static class IocExtensions
{
    #region Service

    /// <summary>
    /// 将指定服务添加到服务容器中。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="singletonMode">true，则启用单例模式；否则为 false。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService>(this IIocContainer container, bool singletonMode = false, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), singletonMode, promote);
        return container;
    }

    /// <summary>
    /// 将指定服务添加到服务容器中。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <typeparam name="TActual">实际的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="singletonMode">true，则启用单例模式；否则为 false。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService, TActual>(this IIocContainer container, bool singletonMode = false, bool promote = false)
        where TActual : TService
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), typeof(TActual), singletonMode, promote);
        return container;
    }

    /// <summary>
    /// 将指定服务添加到服务容器中。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="serviceInstance">要添加的服务的实例。 此对象必须实现 <typeparamref name="TService"/> 参数所指示的类型或从其继承。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService>(this IIocContainer container, TService serviceInstance, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), serviceInstance, promote);
        return container;
    }

    /// <summary>
    /// 将指定服务添加到服务容器中。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="callback">用于创建服务的回调对象。这允许将服务声明为可用，但将对象的创建延迟到请求该服务之后。</param>
    /// <param name="singletonMode">true，则启用单例模式；否则为 false。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService>(this IIocContainer container, InstanceCreatorCallback callback, bool singletonMode = false, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), callback, singletonMode, promote);
        return container;
    }

    /// <summary>
    /// 从服务容器中移除指定的服务类型。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Remove<TService>(this IIocContainer container, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Remove(typeof(TService), promote);
        return container;
    }

    /// <summary>
    /// 获取指定类型的服务对象。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="lastMappingValues">后期映射的参数值数组。请保证数组顺序与构造函数的后期映射的参数顺序一致。</param>
    /// <returns><typeparamref name="TService"/> 类型的服务对象。- 或 -如果没有 <typeparamref name="TService"/>> 类型的服务对象，则为默认值。</returns>
    public static TService Get<TService>(this IIocContainer container, params object[] lastMappingValues)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        var service = container.Get(typeof(TService), lastMappingValues);
        if(service == null) return default(TService);
        return (TService)service;
    }

    /// <summary>
    /// 从手工注册服务列表中，获取指定类型的服务对象。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="lastMappingValues">后期映射的参数值数组。请保证数组顺序与构造函数的后期映射的参数顺序一致。</param>
    /// <returns><typeparamref name="TService"/> 类型的服务对象。- 或 -如果没有 <typeparamref name="TService"/>> 类型的服务对象，则为默认值。</returns>
    public static TService GetFixed<TService>(this IIocContainer container, params object[] lastMappingValues)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        var service = container.GetFixed(typeof(TService), lastMappingValues);
        if(service == null) return default(TService);
        return (TService)service;
    }

    /// <summary>
    /// 获取指定类型的所有服务对象。
    /// </summary>
    /// <typeparam name="TService">一个对象，它指定要获取的服务对象的类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="lastMappingValues">后期映射的参数值数组。请保证数组顺序与构造函数的后期映射的参数顺序一致。</param>
    /// <returns><typeparamref name="TService"/> 类型的所有服务对象。</returns>
    public static IEnumerable<TService> GetAll<TService>(this IIocContainer container, params object[] lastMappingValues)
        => container.GetAll(typeof(TService), lastMappingValues).Cast<TService>();

    /// <summary>
    /// 查找服务容器是否包含指定的服务类型。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>如果存在返回 true，否则返回 false。</returns>
    public static bool Contains<TService>(this IIocContainer container, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        return container.Contains(typeof(TService), promote);
    }

    #endregion

    #region TypeValue

    /// <summary>
    /// 将指定的参数名和参数值添加到服务容器中，并绑定到关联的服务类型的构造函数。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="name">参数名称。</param>
    /// <param name="value">参数值。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService>(this IIocContainer container, string name, object value, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), name, value, promote);
        return container;
    }
    /// <summary>
    /// 将指定的参数名和参数值添加到服务容器中，并绑定到关联的服务类型的构造函数。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>>
    /// <param name="name">参数名称。</param>
    /// <param name="callback">用于创建参数的回调对象。这允许将参数声明为可用，但将值的创建延迟到请求该参数之后。</param>
    /// <param name="singletonMode">true，则启用单例模式；否则为 false。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Add<TService>(this IIocContainer container, string name, InstanceCreatorCallback callback, bool singletonMode = false, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Add(typeof(TService), name, callback, singletonMode, promote);
        return container;
    }

    /// <summary>
    /// 从服务容器中移除指定关联的服务类型的参数。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="name">要移除的参数名称。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer Remove<TService>(this IIocContainer container, string name, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        container.Remove(typeof(TService), name, promote);
        return container;
    }

    /// <summary>
    /// 获取指定关联的服务类型和参数名称的值。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="name">参数名称。</param>
    /// <param name="lastMappingValues">后期映射的参数值数组。</param>
    /// <returns>参数名称的值。- 或 -如果没有参数名称的值，则为 null 值。</returns>
    public static object Get<TService>(this IIocContainer container, string name, params object[] lastMappingValues)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        return container.Get(typeof(TService), name, lastMappingValues);
    }
    /// <summary>
    /// 查找服务容器是否包含指定关联的服务类型指定的参数。
    /// </summary>
    /// <typeparam name="TService">要添加的服务类型。</typeparam>
    /// <param name="container">服务容器。</param>
    /// <param name="name">要查找的参数名称。</param>
    /// <param name="promote">true，则将此请求提升到任何父服务容器；否则为 false。</param>
    /// <returns>如果存在返回 true，否则返回 false。</returns>
    public static bool Contains<TService>(this IIocContainer container, string name, bool promote = false)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));

        return container.Contains(typeof(TService), name, promote);
    }

    #endregion

    /// <summary>
    /// 添加或覆盖一个预期服务类型。
    /// </summary>
    /// <typeparam name="TExpect">预期服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>类型服务的绑定器。</returns>
    public static ITypeServiceBinder Use<TExpect>(this IServiceBuilder binder) => binder.Use(typeof(TExpect));
    /// <summary>
    /// 添加一个预期服务类型。
    /// </summary>
    /// <typeparam name="TExpect">预期服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>类型服务的绑定器。</returns>
    public static ITypeServiceBinder UseRange<TExpect>(this IServiceBuilder binder) => binder.UseRange(typeof(TExpect));
    /// <summary>
    /// 添加或覆盖一个值服务。
    /// </summary>
    /// <typeparam name="TExpect">预期服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <param name="name">值服务的参数名称。</param>
    /// <returns>类型服务的绑定器。</returns>
    public static IValueServiceBinder Use<TExpect>(this IServiceBuilder binder, string name) => binder.Use(typeof(TExpect), name);

    /// <summary>
    /// 绑定为短暂模式的服务。
    /// </summary>
    /// <typeparam name="TActual">实际的服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>服务构建器。</returns>
    public static IServiceBuilder Transient<TActual>(this ITypeServiceBinder binder) => binder.Transient(typeof(TActual));
    /// <summary>
    /// 绑定为单例模式的服务。
    /// </summary>
    /// <typeparam name="TActual">实际的服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>服务构建器。</returns>
    public static IServiceBuilder Singleton<TActual>(this ITypeServiceBinder binder) => binder.Singleton(typeof(TActual));
    /// <summary>
    /// 绑定为范围模式的服务。
    /// </summary>
    /// <typeparam name="TActual">实际的服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>服务构建器。</returns>
    public static IServiceBuilder Scoped<TActual>(this ITypeServiceBinder binder) => binder.Scoped(typeof(TActual));
    /// <summary>
    /// 绑定为智能模式的服务。根据 <see cref="IServiceBinder.ExpectType"/> 的特性创建不同模式的服务（默认为短暂模式）。
    /// </summary>
    /// <typeparam name="TActual">实际的服务类型。</typeparam>
    /// <param name="binder">绑定器。</param>
    /// <returns>服务构建器。</returns>
    public static IServiceBuilder As<TActual>(this ITypeServiceBinder binder) => binder.As(typeof(TActual));

    #region AutoMap

    /// <summary>
    /// 指定筛选器，自动映射类型。
    /// </summary>
    /// <param name="container">服务容器。</param>
    /// <param name="mapFilter">依赖注入与控制反转的映射筛选器。</param>
    /// <param name="expectTypeHandler">找到预期类型时发生。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer AutoMap(this IIocContainer container, IMapFilter mapFilter, Action<Type> expectTypeHandler = null)
    {
        if(container == null) throw new ArgumentNullException(nameof(container));
        if(mapFilter == null) throw new ArgumentNullException(nameof(mapFilter));

        var allTypes = ObjectFactory.AllTypes;
        var hasHandler = expectTypeHandler != null;
        foreach(var item in allTypes)
        {
            foreach(var expectType in item.Value)
            {
                var ns = expectType.Namespace;
                if(mapFilter.IsExpectType(expectType))
                {
                    var actualType = mapFilter.FindActualType(allTypes, expectType);
                    if(actualType == null)
                        throw new Exception("无法找到预期定义类型“" + expectType.AssemblyQualifiedName + "”的实际映射类型。");
                    container.Add(expectType, actualType, mapFilter.IsSingletonMode(expectType, actualType));
                    if(hasHandler) expectTypeHandler(expectType);
                    break;
                }
            }
        }
        return container;
    }

    /// <summary>
    /// 指定配置文件“configuration/appSettings”的键，自定映射类型。
    /// </summary>
    /// <param name="container">服务容器。</param>
    /// <param name="appSettingKey">配置文件“configuration/appSettings”的键。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer AutoMapFromConfig(this IIocContainer container, string appSettingKey)
    {
        var namespaceExpression = System.Configuration.ConfigurationManager.AppSettings[appSettingKey];
        if(string.IsNullOrWhiteSpace(namespaceExpression))
            throw new System.Configuration.SettingsPropertyNotFoundException("configuration/appSettings/add[key='" + appSettingKey + "'] 不存在。");
        AutoMap(container, new MapFilter(namespaceExpression));
        return container;
    }
    /// <summary>
    /// 指定命名空间表达式，自动映射类型。
    /// </summary>
    /// <param name="container">服务容器。</param>
    /// <param name="namespaceExpression">筛选器的命名空间表达式。可以是一个完整的命名空间，也可以是“*”起始，或者以“*”结尾。符号“*”只能出现一次。通过“|”可以同时包含多个命名空间。</param>
    /// <param name="actualTypeFullNameFormat">获取或设置筛选器的实际类型的完全限定名的格式项，例如“{0}.Default{1}”，索引 0 表示 - 预期定义接口 - 的命名空间，索引 1 表示 - 预期定义接口 - 的名称（已去 I）。</param>
    /// <returns>服务容器。</returns>
    public static IIocContainer AutoMap(this IIocContainer container, string namespaceExpression, string actualTypeFullNameFormat = null)
    {
        if(string.IsNullOrWhiteSpace(namespaceExpression)) throw new ArgumentNullException(nameof(namespaceExpression));
        return AutoMap(container, new MapFilter(namespaceExpression) { ActualTypeFullNameFormat = actualTypeFullNameFormat });
    }
    /*
    /// <summary>
    /// 指定契约域，自动映射类型。
    /// </summary>
    /// <param name="container">服务容器。</param>
    /// <param name="domain">契约域。</param>
    public static void AutoMap(this IIocContainer container, Aoite.ServiceModel.ContractDomain domain)
    {
        var mapFilter = domain.CreateMapFilter();
        var allTypes = ObjectFactory.AllTypes;
        foreach(var groupItem in allTypes)
        {
            foreach(var expectType in groupItem)
            {
                var ns = expectType.Namespace;
                if(mapFilter.IsExpectType(expectType))
                {
                    var info = domain.AddOrGet(expectType);
                    container.AddService(expectType, lastMappingArguments =>
                    {
                        var keepAlive = false;
                        if(lastMappingArguments.Length == 1 && lastMappingArguments[0] is Boolean)
                            keepAlive = (bool)lastMappingArguments[0];
                        return info.CreateProxyObject(domain, keepAlive);
                    }, false);
                }
            }
        }
    }
    */
    #endregion
}