mergeInto(LibraryManager.library, {
  PlayableOpenStore: function (urlPtr) {
    var url = UTF8ToString(urlPtr);
    if (typeof mraid !== "undefined" && mraid.open) {
      mraid.open(url);
    } else {
      window.open(url, "_blank");
    }
  }
});