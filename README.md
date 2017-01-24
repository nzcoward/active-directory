# Active Directory Helpers

This library presents some wrappers for common AD objects. It simplifies searching and reading properties. 

## Search Results

Search results are wrapped in a particular type of result (group, user, computer) to provide shortcut access to properies of the underlying <em>SearchResult</em>. As your directory objects may have different properties, some may simply be missing, in which case, fork and add. I have also not implemnted all properties from my test directory, simply because there's so many, and I don't need them. 

## The ambient DirectoryEntry

I have chosen to use a <em>ThreadStatic</em> <em>DirectoryEntry</em> for instances where a <em>DirectoryEntry</em> is required. This is so that any methods or properties that require a <em>DirectoryEntry</em> can share calls to the same <em>DirectoryEntry</em> object, without the expense of reloading it - although most calls are InvokeGet, which will execute on the server regardless. The locking mechanism is just static and shared across threads. I don't foresee an issue with this, as performance isn't exactly something one can expect from connections to AD.

## Usage

Easy as:

```cs
var finder = new UserFinder();
var result = finder.Find("my.username");
var passwordExpiration = result.PasswordExpirationDate;
```

You may need to tweak your constructors as the root DirectoryEntry creation for the intial search is very rudimentary.

### Credits

This CodeProject article was the original base of the work: 
https://www.codeproject.com/articles/18102/howto-almost-everything-in-active-directory-via-c
