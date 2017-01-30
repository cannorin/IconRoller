namespace IconRoller
open System
open Suave
open Suave.Html

module View =
  begin
    let info s = 
      [
        "<h1 id=\"info\">Info</h1>";
        sprintf "<p>%s</p>" s;
        "<h1 id=\"menu\">Menu</h1>";
        "<p><a href=\"/twitter_login\">Login</a>";
        "<a href=\"/logout\">Logout</a></p>" 
      ] 
      |> String.concat "\n"
  end