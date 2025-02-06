# SecureDelete
This is a program that securely deletes files.  I know, I know, there's like a bazillion of those already, but
have you ever found yourself wondering, especially if such a program is written by coders in countries with,
let's say, "less than savory reputations"?  Yeah, I know.  I'm a bad man for thinking that.  Given I'm white
American, and Christian, I'm already public enemy number 1, so what do I have to lose at this point?

I want a program for this that I 100% trust and I don't want to pay for something.

So yeah, this just takes a file name in, encrypts it but never saves the key, and then deletes the file.  This
gets you around most forensics and considerations like wear-leveling algos on SSDs.  It isn't fancy.  One file
at a time, no wildcards, no folders.

