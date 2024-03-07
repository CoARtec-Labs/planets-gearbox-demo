using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Core.Editor;
using System.Text.RegularExpressions;


namespace AndroidBuildExtensions
{
static class Extensions
{

    private static string Replace(this string text, string pattern, string replacement, int count)
    {
        if (count == 0) return text;
        var rgx = new Regex(pattern);
        if (count < 0) return rgx.Replace(text, replacement);
    
        return rgx.Replace(text, replacement, count);
    }

    

    internal static string RegexReplace(this string text, string pattern, string replacement, int count = -1)
    {
        
        if (replacement.Length <= 10)
        {
            return text.Replace(pattern, replacement, count);
        }
        

        var tempReplacement = replacement.RegexReplace(@"\$\d+", ""); // Filter out regex substitution variables
        bool isMatch;
        if (text.Contains(tempReplacement))
        {

            isMatch = true;
        } else 
        {

            isMatch = false;
        }
    
        if (isMatch) 
        {
            return text; // Only modify text, if it is not already present in text!
        }
        return text
            .Replace(pattern, replacement, count);
    }


    internal static string GetPathToFileOrDir(this string path, string relativePathToFile)
    {
        return path.Replace("unityLibrary", relativePathToFile);

    }


    }

    static class ManifestXmlExtensions
    {

        internal static string DeletePackageInfo(this string text)
        {
            return text.RegexReplace(@"\spackage=\u0022[\S+\.]{3,}\S+\u0022", "");
        }

        internal static string ReplaceUnityPackageName(this string text, string packageName)
        {
            return text.Replace("com.unity3d.player", packageName);
        }

        internal static string DeleteIntentFilter(this string text)
        {
            return text.RegexReplace(@"<intent-filter>(?:.*\n){3}.*</intent-filter>", "");;
        }

        internal static string EditXmlTagValue(this string text, string tag, string newValue)
        {
            var pattern = $@"({tag}\u003d\u0022)\w+(\u0022)";
            // Debug.Log($"EditXmlTagValue: pattern={pattern}");
            return text.RegexReplace($@"({tag}=\u0022)\w+(\u0022)", $"$1{newValue}$2");
        }

        internal static string EditXmlTagValues(this string text, Dictionary<string, string> tagValues)
        {
            var result = text;
            foreach(var tagValuePair in tagValues)
            {
                result = result.EditXmlTagValue(tagValuePair.Key, tagValuePair.Value);
            }
            return result;
        }


        internal static string EditStylesTheme(this string text)
        {
            return text.RegexReplace(@"(name=\u0022BaseUnityTheme\u0022\sparent\u003d\u0022)[^\u0022\s]*(\u0022)", "$1Theme.AppCompat.Light.NoActionBar$2");
        }

    }

    static class BuildGradleList
    {

        internal static LinkedList<string> FilterOutObsoleteInfos(this LinkedList<string> list, List<string> keyWords)
        {
            foreach(var keyWord in keyWords)
            {
                
                list = list.RemoveAll((node) => {
                    // Debug.Log($"FilterOutObsoleteInfos: matchLambda: keyWord={keyWord}");
                    return node.Contains(keyWord);
                });
            }
            return list;
        }

        internal static LinkedList<T> RemoveAll<T>(this LinkedList<T> list, Predicate<LinkedListNode<T>> match, bool onlyFirstMatch = true)
    {
        if (list == null)
        {
            throw new ArgumentNullException("list");
        }
        if (match == null)
        {
            throw new ArgumentNullException("match");
        }
        var count = 0;
        var node = list.First;
        while (node != null)
        {
            var next = node.Next;
            // Debug.Log($"RemoveAll: node={node.Value}, match={match(node)}");
            if (match(node))
            {
                list.Remove(node);
                if (onlyFirstMatch)
                {
                    return list;
                }
                count++;

            }
            node = next;
        }
        return list;
    }
    internal static LinkedList<string> ToLinkedList(this string text)
        {
            return new LinkedList<string>(text.Split("\n"));
        }

        internal static string ToText(this LinkedList<string> list)
        {
            var text = "";
            var currentLine = list.First;
            while(currentLine != null)
            {
                text += currentLine.Value + "\n";
                currentLine = currentLine.Next;
            }
            return text;
        }



        internal static LinkedList<string> AddPlugins(this LinkedList<string> list, IEnumerable<string> plugins)
        {
            return list.AddAndroidConfigurations(
                "plugins",
                "id",
                plugins,
                list.First
            );
        }

        internal static LinkedList<string> AddDependencies(this LinkedList<string> list, IEnumerable<string> dependencies)
        {
            return list.AddAndroidConfigurations(
                
                "dependencies",
                "implementation",
                dependencies,
                list.First.GoToClosure(ref list, "plugins", list.First).SkipToEndOfClosure().Next
            );
            
        }

        internal static LinkedList<string> AddAndroidConfigurations(
            this LinkedList<string> list, 
            string closure,
            string key,
            IEnumerable<string> configurations,
            LinkedListNode<string> nodeToAddNewClosure
            )
        {
            return list.AddAndroidConfiguration(
                closure,
                configurations,
                (entry) => $"{key} {entry}",
                nodeToAddNewClosure,
                (node) => node.SkipToEndOfClosure(),
                (node) => entry => node.Contains(entry)
            );
            
        }

        internal static LinkedList<string> AddAndroidConfiguration(
        this LinkedList<string> list, 
        Dictionary<string, List<KeyValuePair<string, string>>> configs
        )
        {
            foreach (var configPair in configs)
            {
                // Debug.Log($"AddAndroidConfiguration: config Key={configPair.Key}, config Value={configPair.Value}");
                list = list.AddAndroidConfiguration(
                    configPair.Key,
                    configPair.Value
                );
            }
            return list;
        }

    internal static LinkedList<string> AddAndroidConfiguration(
        this LinkedList<string> list, 
        string closure, 
        IEnumerable<KeyValuePair<string, string>> configs
        )
        {
            var closures = closure.Split('.');
            var depth = closures.Length - 1;
            var nestedClosure = closures.Last();
            var currentNode = list.First.GoToClosure(ref list, "dependencies", list.First.SkipToEndOfClosure().Next);
            if (currentNode != null)
            {
                currentNode = currentNode.SkipToEndOfClosure().Next;
            } else
            {
                currentNode = list.First;
            }
            LinkedListNode<string> nodeToAddNewClosure;
            if (depth > 0)
            {
                nodeToAddNewClosure = currentNode.GoToClosure(ref list, closures[^2], currentNode).SkipToEndOfClosure();
            } else
            {
                nodeToAddNewClosure = currentNode;
            }
            
            return list.AddAndroidConfiguration(
                nestedClosure,
                configs,
                (pair) => $"{pair.Key} {pair.Value}",
                nodeToAddNewClosure,
                (node) => node.Next,
                (node) => (entry) => node.Contains(entry.Key),
                (node) => (newConfig) => node.Value = node.Value.RegexReplace($@"^([^\S\r\n]*)\w+.*$", $"$1{newConfig.Key} {newConfig.Value}"),
                depth
            );
        }

    internal static LinkedList<string> AddAndroidConfiguration<T>(
        this LinkedList<string> list, 
        string closure, 
        IEnumerable<T> configs,
        Func<T, string> convert,
        LinkedListNode<string> nodeToAddNewClosure,
        Func<LinkedListNode<string>, LinkedListNode<string>> GetNodeToAddNewConfigs,
        Func<LinkedListNode<string>, Func<T, bool>> match,
        Func<LinkedListNode<string>, Action<T>> executeOn = null,
        int depth = 0
        )
        {
            var closureNode = list.First.GoToClosure(ref list, closure, nodeToAddNewClosure, depth);
            // Debug.Log($"AddAndroidConfiguration<T>: closureNode={closureNode.Value}");
            configs = configs.RemoveAllIfPresentAndExecute(closureNode.Next, convert, match, executeOn);
            var currentNode = GetNodeToAddNewConfigs(closureNode);
            // Debug.Log($"AddAndroidConfiguration<T>: currentNode={currentNode.Value}");

            return list.AddLinesBefore(currentNode, configs, convert, depth + 1);
        }

        internal static LinkedListNode<string> GoToClosure(
            this LinkedListNode<string> currentNode,
            ref LinkedList<string> list, 

            string closure, 
            LinkedListNode<string> nodeToAddNewClosure,
            int depth = 0
            )
        {
            var closureNode = currentNode.FindClosure(closure);
            // Debug.Log($"GoToClosure: closure={closure}, is closureNode null={closureNode == null}, is nodeToAddNewClosure null={nodeToAddNewClosure==null}");
            if (closureNode == null)
            {
                // Debug.Log($"GoToClosure: AddNewClosure: closure={closure},closureNode={closureNode}, nodeToAddNewClosure={nodeToAddNewClosure}");

                list = list.AddClosure(ref nodeToAddNewClosure, closure, depth);
                closureNode = nodeToAddNewClosure;
                // Debug.Log($"GoToClosure: closure={closure},closureNode={closureNode.Value}, nodeToAddNewClosure={nodeToAddNewClosure}");

            }
            
            return closureNode;
        }
        internal static LinkedListNode<string> SkipToEndOfClosure(this LinkedListNode<string> node)
        {
            

            return node.ExecuteInsideClosure(node, (currentNode, _) => 
            {
                if (currentNode != null)
                {
                    // Debug.Log($"SkipToEndOfClosure: currentNode={currentNode.Value}");
                } else
                {
                    // Debug.Log($"SkipToEndOfClosure: currentNode={null}");

                }
                return currentNode;
        });

        }

        internal static int CurrentClosureDepth(this LinkedListNode<string> node, int seed = 0)
        {
            if (node == null)
            {
                return -1; // Break out of loop
            }
            return seed + node.Value.Count((c) => c == '{') - node.Value.Count((c) => c == '}');
        }

    internal static T ExecuteInsideClosure<T>(this LinkedListNode<string> node, T value, Func<LinkedListNode<string>, int, T> func)
        {
            var initialDepth = node.CurrentClosureDepth();
            if (initialDepth < 0)
            {
                return value;
            } else if (initialDepth == 0)
            {
                value = func(node, initialDepth);
            }
            var depth = initialDepth;
            var nextNode = node.Next;
            while (depth >= initialDepth && nextNode != null)
            {
                node = nextNode;
                value = func(node, depth);
                depth = node.CurrentClosureDepth(depth);
                nextNode = node.Next;
            } 



            return value;

        }    

        internal static IEnumerable<T> RemoveAllIfPresentAndExecute<T>(
            this IEnumerable<T> list, 
            LinkedListNode<string> node,
            Func<T, string> convert, 
            Func<LinkedListNode<string>, Func<T, bool>> match,
            Func<LinkedListNode<string>, Action<T>> executeOn
            )
        {
            var initialDepth = node.CurrentClosureDepth();
            
            return node.ExecuteInsideClosure(list, (currentNode, depth) =>
            {
                if (depth == initialDepth && currentNode.Value.IsNotNullOrWhiteSpace())
                {
                    // Debug.Log($"RemoveAllIfPresentAndExecute: currentNode={currentNode}, depth = {depth}, executeOn={executeOn}");
                    list = list.RemoveIfPresentAndExecute(convert, match(currentNode), executeOn?.Invoke(currentNode));
                }
                return list;
            });
        }

        internal static LinkedListNode<string> FindClosure(this LinkedListNode<string> node, string closure)
        {
            while (node != null && !node.ContainsClosure(closure))
            {
                node = node.Next;
            }

            return node;
        }

        internal static IEnumerable<T> RemoveIfPresentAndExecute<T>(
            this IEnumerable<T> list, 
            Func<T, string> convert, 
            Func<T, bool> match, 
            Action<T> executeOn = null
            )
            {
                
                return list.RemoveIfMatch((entry) => 
                {
                    if (match(entry))
                    {
                        executeOn?.Invoke(entry);
                        return true;
                    }
                    return false;
                });
            }

        internal static IEnumerable<T> RemoveIfMatch<T>(this IEnumerable<T> list, Func<T, bool> match)
        {
            return list.Where((entry) => !match(entry));

        }

        internal static LinkedList<string> AddLinesBefore<T>(this LinkedList<string> list, LinkedListNode<string> node, IEnumerable<T> values, Func<T, string> convert, int depth = 0)
        {
            if (values == null || values.Count() == 0)
            {
                return list;
            }
            foreach (var value in values)
            {
                list = list.AddLineBefore(node, value, convert, depth);

            }
            return list;
        }


        internal static LinkedList<string> AddLineBefore(this LinkedList<string> list, LinkedListNode<string> node, string value, int depth = 0)
        {
            
            
            return list.AddLineBefore(node, value, (_) => value, depth);
        }

        internal static LinkedList<string> AddLineBefore<T>(this LinkedList<string> list, LinkedListNode<string> node, T value, Func<T, string> convert, int depth = 0)
        {
            var tabs = new string('\t', depth);
            if (node == null)
            {
                // Debug.LogError("AddLineBefore: node is null!");
                node = list.Last;
            }
            var newValue = $"{tabs}{convert(value)}";
            // Debug.Log($"AddLineBefore: new line= {newValue}");

            list.AddBefore(node, $"{tabs}{convert(value)}");        
            return list;
        }



        internal static LinkedList<string> AddClosure(this LinkedList<string> list, ref LinkedListNode<string> node, string closure, int depth = 0)
        {
            list = list.AddLineBefore(node, $"{closure}" + " {", depth);
            list = list.AddLineBefore(node, "}", depth);
            node = node.Previous.Previous;
            // Debug.Log($"AddClosure: node={node.Value}");
            return list;
        }


        internal static bool ContainsClosure(this LinkedListNode<string> node, string closure)
            {
                return node.Contains(closure, true);
            }
        internal static bool Contains(this LinkedListNode<string> node, string keyWord, bool isClosure = false)
        {
            keyWord = keyWord.RegexReplace(@"([\(\)\$\u0022])", "\\$1");
            string pattern;
            if (isClosure)
            {
                pattern = $@"^[^\S\r\n]*{keyWord}" + @"[^\S\r\n]\{.*$";
            } else
            {
                pattern = $@"^.*{keyWord}" + @"(?![^\S\r\n]\{$).*$";
            }
            return Regex.IsMatch(node.Value, pattern);
        }
    }

}


