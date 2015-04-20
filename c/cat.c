/*
 * Copyright (c) 2015 Brian Callahan <bcallah@openbsd.org>
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

#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

int
main(int argc, char *argv[])
{
	FILE           *fp;
	char 		c;
	int 		ch;

	/*
	 * POSIX only specifies a -u flag.
	 */
	while ((ch = getopt(argc, argv, "u")) != -1)
		switch (ch) {
		case 'u':
			setbuf(stdout, NULL);
			break;
		default:
			(void) fprintf(stderr, "usage: cat [-u] [file ...]\n");
			exit(1);
			/* NOTREACHED */
		}
	argc -= optind;
	argv += optind;

	/*
	 * Handle the case where no files are specified separately.
	 */
	if (argc < 1) {
		while ((c = fgetc(stdin)) != EOF)
			fputc(c, stdout);
		goto out;
	}

	while (*argv) {
		if ((strcmp(*argv, "-")) == 0) {
			while ((c = fgetc(stdin)) != EOF)
				fputc(c, stdout);
			*++argv;
			continue;
		}
		if ((fp = fopen(*argv, "r")) == NULL) {
			(void) fprintf(stderr, "cat: %s: %s\n", *argv++, strerror(errno));
			continue;
		}

		while ((c = fgetc(fp)) != EOF)
			fputc(c, stdout);

		fclose(fp);
		*++argv;
	}

out:
	return 0;
}