/**
Copyright (c) 2014, Michael Notarnicola
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSGTools {
	namespace Utilities {

		public static class GUIUtils {
			public static void InsertTab(int tablevel) {
				GUILayout.Space(16 * tablevel);
			}
		}

		public static class ComponentExtensions {
			public static T GetAddComponent<T>(this Component component) where T : Component {
				T result = component.GetComponent<T>();
				if(result == null)
					result = component.gameObject.AddComponent<T>();
				return result;
			}

			public static bool HasComponent<T>(this Component component) where T : Component {
				return component.GetComponent<T>() != null;
			}

			public static T GetAddComponent<T>(this GameObject go) where T : Component {
				T result = go.GetComponent<T>();
				if(result == null)
					result = go.AddComponent<T>();
				return result;
			}

			public static bool HasComponent<T>(this GameObject go) where T : Component {
				return go.GetComponent<T>() != null;
			}
		}

		public static class TransformUtils {

			public static IEnumerable<Transform> ChildrenEnumerable(this Transform transform) {
				return transform.Cast<Transform>();
			}

			public static void SetHierarchyPosition(this Transform child, Transform target) {
				SetHierarchyPosition(child, target.position);
			}

			public static void SetHierarchyPosition(this Transform child, Vector3 target) {
				child.root.position = target;
				child.root.position += target - child.position;
			}

			public static void SetHierarchyForward(this Transform child, Transform target) {
				SetHierarchyForward(child, target.forward);
			}

			public static void SetHierarchyForward(this Transform child, Vector3 target) {
				child.root.rotation = Quaternion.LookRotation(target);
				child.root.rotation *= Quaternion.Inverse(child.rotation) * child.root.rotation;
			}

			public static void SetHierarchyRotation(this Transform child, Transform target) {
				SetHierarchyRotation(child, target.rotation);
			}

			public static void SetHierarchyRotation(this Transform child, Vector3 target) {
				SetHierarchyRotation(child, Quaternion.Euler(target));
			}

			public static void SetHierarchyRotation(this Transform child, Quaternion target) {
				child.root.rotation = target;
				child.root.rotation *= Quaternion.Inverse(child.rotation) * child.root.rotation;
			}

			public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
				Vector3 position = transform.position;
				if(x.HasValue)
					position.x = x.Value;
				if(y.HasValue)
					position.y = y.Value;
				if(z.HasValue)
					position.z = z.Value;
				transform.position = position;
			}

			public static void SetRotation(this Transform transform, float? x = null, float? y = null, float? z = null) {
				Vector3 rotation = transform.rotation.eulerAngles;
				if(x.HasValue)
					rotation.x = x.Value;
				if(y.HasValue)
					rotation.y = y.Value;
				if(z.HasValue)
					rotation.z = z.Value;
				transform.rotation = Quaternion.Euler(rotation);
			}
		}

		public static class AudioUtils {
			public static void PlayAudioHere(this GameObject gameObject, AudioClip clip) {
				AudioSource.PlayClipAtPoint(clip, gameObject.transform.position);
			}

			public static void PlayAudioHere(this GameObject gameObject, AudioClip clip, float volume) {
				AudioSource.PlayClipAtPoint(clip, gameObject.transform.position, volume);
			}
		}

		public static class GenericExtensions {

			/// <summary>
			/// The same as Remove, except it returns the item removed
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="list"></param>
			/// <param name="index"></param>
			/// <returns></returns>
			public static T Retrieve<T>(this IList<T> list, int index) {
				T item = list[index];
				list.RemoveAt(index);
				return item;
			}

			public static void ModifyIndex(this IList list, int index, int modifier) {
				int modifiedIndex = index + modifier;
				if(modifiedIndex < 0 || modifiedIndex >= list.Count)
					return;
				object item = list[index];
				list.RemoveAt(index);
				list.Insert(modifiedIndex, item);
			}

			public static bool IsNull<T>(this T obj) {
				return obj == null || obj.Equals(null);
			}
		}

		public static class MathUtils {
			public static float ClampAngle(float angle) {
				if(angle < 0f)
					return angle + (360f * (int)((angle / 360f) + 1));
				else if(angle > 360f)
					return angle - (360f * (int)(angle / 360f));
				else
					return angle;
			}

			public static int ClampAngle(int angle) {
				if(angle < 0)
					return angle + (360 * ((angle / 360) + 1));
				else if(angle > 360)
					return angle - (360 * (angle / 360));
				else
					return angle;
			}
		}

		public static class StringExtensions {
			public static bool IsNullOrEmpty(this string str) {
				return string.IsNullOrEmpty(str);
			}

			public static bool IsNullOrWhitespace(this string str) {
				return string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim());
			}
		}

		public static class LINQExtensions {
			public static T Random<T>(this IEnumerable<T> enumerable) {
				return enumerable.ElementAtOrDefault(UnityEngine.Random.Range(0, enumerable.Count()));
			}

			public static bool ContainsAny<T>(this IEnumerable<T> enumerable, IEnumerable<T> from) {
				foreach(T t in from)
					if(enumerable.Contains(t))
						return true;

				return false;
			}

			public static bool ContainsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> from) {
				foreach(T t in from)
					if(!enumerable.Contains(t))
						return false;
				return true;
			}

			public static T Mode<T>(this IEnumerable<T> enumerable) {
				var groups = enumerable.GroupBy(v => v);
				int maxCount = groups.Max(g => g.Count());
				return groups.First(g => g.Count() == maxCount).Key;
			}
		}

		public static class EnumExtensions {
			/// <summary>
			/// Check to see if a flags enumeration has a specific flag set.
			/// </summary>
			/// <param name="variable">Flags enumeration to check</param>
			/// <param name="value">Flag to check for</param>
			/// <returns></returns>
			public static bool HasFlag(this Enum variable, Enum value) {
				if(variable == null)
					return false;

				if(value == null)
					throw new ArgumentNullException("value");

				// Not as good as the .NET 4 version of this function, but should be good enough
				if(!Enum.IsDefined(variable.GetType(), value)) {
					throw new ArgumentException(string.Format(
						"Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
						value.GetType(), variable.GetType()));
				}

				ulong num = Convert.ToUInt64(value);
				return ((Convert.ToUInt64(variable) & num) == num);
			}

		}
	}
}