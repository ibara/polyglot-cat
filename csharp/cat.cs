/*
 * Copyright (c) 2015 Scott Bennett
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * 
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.IO;

namespace cat {

	/// <summary>
	/// cat - A reimplimentation of the Unix utility in C#
	/// 
	/// This program takes a list of files as its arguments and prints the 
	/// contents to stdout.
	/// 
	/// Note: This program is not POSIX compliant.
	/// </summary>
	public class cat {

		/// <summary>
		/// Attempt to read and print the contents of the specified file.
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args) {
			if (args.Length < 1) {
				Usage();
				return;
			}

			foreach (string arg in args) {
				try {
					string filePath = arg;

					if (!File.Exists(filePath)) {
						Console.WriteLine(
							string.Format("cat: {0} does not exist", filePath)
						);
						continue;
					}

					/* 
					 * Wrap the stream declaration in a using() statement so that 
					 * it will be disposed of properly.
					 */
					using (FileStream stream = File.OpenRead(filePath)) {
						bool keepReadingFromFile = true;
						int characterByte;

						/* Read the contents of the file one byte at a time. */
						do {
							characterByte = stream.ReadByte();

							/* 
							 * ReadByte() returns -1 when the end of the stream 
							 * is reached.
							 */
							if (characterByte < 0) {
								keepReadingFromFile = false;
							} else {
								/* Print the character. */
								Console.Write(char.ConvertFromUtf32(characterByte));
							}
						} while (keepReadingFromFile);
					}
				}
				catch (Exception e) {
					Console.WriteLine("cat: Error: " + e.Message);
				}
			}
		}

		/// <summary>
		/// Print out the usage of this program.
		/// </summary>
		public static void Usage() {
			Console.WriteLine("usage: cat [file ...]");
		}
	}
}
