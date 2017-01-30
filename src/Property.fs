namespace IconRoller
open System
open System.IO

module Property =
  begin

    let public ConsumerKey () =
      if File.Exists "CKCS" then
        File.ReadLines "CKCS"
        |> Seq.find (fun x -> x.StartsWith "export CK=")
        |> String.split "\"".[0]
        |> Seq.skip 1 |> Seq.head
      else
        System.Environment.GetEnvironmentVariable("CK")

    let public ConsumerSecret () =
      if File.Exists "CKCS" then
        File.ReadLines "CKCS"
        |> Seq.find (fun x -> x.StartsWith "export CS=")
        |> String.split "\"".[0]
        |> Seq.skip 1 |> Seq.head
      else
        System.Environment.GetEnvironmentVariable("CS")

    let public TwitterRedirectPath () =
      if System.Environment.GetEnvironmentVariable("PORT") = null then
        "http://127.0.0.1:8080/twitter_redirect"
      else
        "https://iconroller.herokuapp.com/twitter_redirect"

  end
