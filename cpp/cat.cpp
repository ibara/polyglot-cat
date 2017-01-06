/*
 * Copyright (c) 2015, 2017 Brian Callahan <bcallah@openbsd.org>
 *
 * Permission to use, copy, modify, and distribute this software for any
 * purpose with or without fee is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
 * WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
 * ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
 * ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
 * OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
 */

#include <cerrno>
#include <iostream>
#include <cstdlib>
#include <cstring>
#include <unistd.h>

using namespace std;

int
main(int argc, char *argv[])
{
	FILE   *fp;
	char   *progname;
	int 	c, ch, ret = 0;

	setlocale(LC_ALL, "");
	progname = *argv;

	/*
	 * POSIX only specifies a -u flag.
	 * This is a POSIX compliant cat.
	 */
	while ((ch = getopt(argc, argv, "u")) != -1)
		switch (ch) {
		case 'u':
			setvbuf(stdout, NULL, _IONBF, 0);
			break;
		default:
			cerr << "usage: " << progname << " [-u] [file ...]\n";
			exit(1);
			/* NOTREACHED */
		}
	argc -= optind;
	argv += optind;

	do {
		if (argc == 0 || (strcmp(*argv, "-")) == 0) {
			clearerr(stdin);
			while ((c = getc(stdin)) != EOF)
				cout << c;
			if (argc == 0)
				return 0;
			continue;
		}
		if ((fp = fopen(*argv, "r")) == NULL) {
			cerr << progname << ": " << *argv << " "
			     << strerror(errno) << "\n";
			ret = 1;
			continue;
		}

		while ((c = getc(fp)) != EOF)
			cout << static_cast<char>(c);

		fclose(fp);
	} while (*++argv);

	return ret;
}
