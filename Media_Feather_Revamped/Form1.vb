Imports System.ComponentModel
Imports System.IO

Public Class Form1 : Inherits CustomWindow
    Dim m As New media.Media
    Dim sm As New SongManager
    Dim ext$ = "4mp$669$6cm$8cm$8med$8svx$a2m$aa$aa3$aac$aax$abc$abm$ac3$acd$acm$act$adg$afc$agm$ahx$aif$aifc$aiff$ais$akp$al$alaw$all$amf$amr$ams$ams$aob$ape$apf$ase$atrac$au$aud$aup$avr$awb$band$bap$bdd$box$bun$bwf$c01$caf$cda$cdr$cel$cidb$cmf$copy$cpr$cpt$csh$cwp$d00$d01$dcf$dcm$dct$ddt$dewf$df2$dfc$dig$dig$dls$dm$dmf$dsf$dsm$dsp$dss$dtm$dts$dtshd$dvf$dwd$ear$efa$efe$efk$efq$efs$efv$emd$emp$emx$esps$f2r$f32$f3r$f64$far$fff$flac$flp$fls$fsm$fzb$fzf$fzv$g721$g723$g726$gig$gp5$gpk$gsm$gsm$hma$ics$iff$imf$ins$ins$it$iti$its$jam$k25$k26$kar$kin$kit$kmp$koz$koz$krz$ksc$ksf$kt2$kt3$ktp$l$la$lqt$lso$lvp$lwv$m1a$m3u$m4a$m4b$m4p$m4r$ma1$mdl$med$mgv$mid$midi$miniusf$mka$mlp$mmf$mmm$mo3$mod$mp1$mp2$mp3$mpa$mpc$mpd$mpga$mpu$mp_$msv$mt2$mte$mti$mtm$mtp$mts$mus$mws$mzp$nap$nki$nra$nrt$nsa$nsf$nst$ntn$nwc$odm$oga$ogg$okt$oma$omf$omg$omx$ots$ove$pac$pat$pbf$pca$pcg$pcm$peak$phy$pk$pla$pls$pna$prg$prg$psf$psm$ptf$ptm$pts$pvc$qcp$r$r1m$ra$ram$raw$rax$rbs$rex$rfl$rmf$rmi$rmj$rmm$rmx$rng$rns$rol$rsn$rso$rti$rtm$rts$s3i$s3m$saf$sam$sb$sbi$sbk$sc2$sd$sd$sd2$sds$sdx$seg$ses$sf$sf2$sfk$sfl$shn$sib$sid$sid$smf$smp$snd$snd$snd$sng$sou$sppack$sprg$sseq$ssnd$stm$stx$sty$svx$sw$swa$syh$syw$syx$td0$tfmx$thx$toc$tsp$txw$u$ub$ulaw$ult$ulw$uni$usf$usflib$uw$uwf$vag$val$vc3$vmd$vmf$vmf$voc$voi$vox$vpl$vpm$vqf$vrf$vyf$w01$wav$wav$wave$wax$wfb$wfd$wfp$wma$wow$wpk$wproj$wrk$wus$wut$wv$wvc$wwu$xfs$xi$xm$xmf$xmi$xmz$xp$xrns$xsb$xspf$xt$xwb$zvd$zvr"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Files = Directory.GetFiles("f:\music\", "*.*", 1)
        Dim Extensions = ext.Split("$")
        Dim songs As New List(Of String)
        For Each S In Files
            For Each Tt In Extensions
                If Path.GetExtension(S) = "." & Tt Then : songs.Add(S) : Exit For : End If
            Next
        Next
        sm.LoadSongs(songs)
        Dim td As Dictionary(Of Integer, Song) = sm.GetSongs()
        For Each i% In td.Keys
            ListBox1.Items.Add(td.Values(i).Name)
        Next
    End Sub
    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDoubleClick
        sm.StartPlayback(ListBox1.SelectedIndex)
    End Sub
End Class
